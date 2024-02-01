using AutoMapper;
using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Models.DTO;
using Hospital.Services.Clinic.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Services.Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IPatientAppointment _patientAppointment;
        private readonly IMapper _mapper;

        public AppointmentsController(IPatientAppointment patientAppointment, IMapper mapper)
        {
            _patientAppointment = patientAppointment;
            _mapper = mapper;
        }


     //   [Authorize(Roles = "Admin")]
        [HttpGet("GetAppointments")]
        public async Task<IEnumerable<PatientAppointments>> GetAppointmentAsync()
        {
            return await _patientAppointment.GetAppointmentAsync();
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddAppointment([FromBody] PatientAppointmentsDto appointmentDto)
        {
            if (_patientAppointment.IsPatientAlreadyScheduled(appointmentDto.Pid, appointmentDto.AppointmentDate))
            {

                if (_patientAppointment.IsSerialNoAlreadyAssigned(0,appointmentDto.SerialNo))
                {
                    var appointmentEntity = _mapper.Map<PatientAppointments>(appointmentDto);
                    appointmentEntity.Status = "Pending";
                    var result= await _patientAppointment.AddPatientAppointment(appointmentEntity);

                    if (result.Success)
                    {
                        return Ok(result);
                    }
                    return BadRequest(new { message = "somthing went wrong" });
                }

                return BadRequest(new { success = false, Message = "Serial No is already assigned to another appointment." });
            }
            return BadRequest(new { success = false, Message = "This Patient already has an appointment Schedule for the selected date." });

        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAppointment(int Id,[FromBody] PatientAppointmentsDto appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAppointment = await _patientAppointment.GetAppointmentIdAsync(Id);

            if (existingAppointment != null)
            {
                if (_patientAppointment.IsPatientAlreadyScheduled(appointmentDto.Pid, appointmentDto.AppointmentDate))
                {
                      var map =  _mapper.Map(appointmentDto, existingAppointment);
                    if (_patientAppointment.IsSerialNoAlreadyAssigned(map.Id, map.SerialNo))
                    {
                        await _patientAppointment.UpdatePatientAppointment(existingAppointment);
                        return Ok(new { Message = "Appointment updated successfully." });
                    }
                    return BadRequest(new { success = false, Message = "Serial No is already assigned to another appointment." });
                }
                return BadRequest(new { success = false, Message = "This Patient already has an appointment Schedule for the selected date." });

            }

            return NotFound(new { Message = "Appointment not found." });
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteAppointment(int appointmentId)
        {
            var existingAppointment = await _patientAppointment.GetAppointmentIdAsync(appointmentId);

            if (existingAppointment != null)
            {
                await _patientAppointment.DeletePatientAppointment(appointmentId);
                return Ok(new { Message = "Appointment deleted successfully." });
            }

            return NotFound(new { Message = "Appointment not found." });
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<PatientAppointmentsDto>> GetAppointmentById(int pId)
        {
            var appointment = await _patientAppointment.GetAppointmentIdAsync(pId);

            if (appointment != null)
            {
                var appointmentDto = _mapper.Map<PatientAppointmentsDto>(appointment);
                return Ok(appointmentDto);
            }

            return NotFound(new { Message = "Appointment not found." });
        }

        [HttpPatch("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(int appointmentId, string newStatus)
        {
            try
            {
                var appointment = await _patientAppointment.GetAppointmentIdAsync(appointmentId);

                if (appointment == null)
                {
                    return NotFound(new { Message = "Appointment not found." });
                }

                if (string.IsNullOrEmpty(newStatus))
                {
                    return BadRequest(new { Message = "New status cannot be empty." });
                }

                appointment.Status = newStatus;

                await _patientAppointment.UpdatePatientAppointment(appointment);

                return Ok(new { Message = "Appointment status updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


    }
}
