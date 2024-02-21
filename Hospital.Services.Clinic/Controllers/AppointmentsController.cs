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

            var appointmentEntity = _mapper.Map<PatientAppointments>(appointmentDto);
            appointmentEntity.Status = "Pending";
            var result = await _patientAppointment.AddPatientAppointment(appointmentEntity);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }



        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAppointment(int Id, [FromBody] PatientAppointmentsDto appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAppointment = await _patientAppointment.GetAppointmentIdAsync(Id);

            if (existingAppointment != null)
            {
                var appointmentEntity = _mapper.Map<PatientAppointments>(appointmentDto);
                appointmentEntity.Id = Id;
                var result = await _patientAppointment.UpdatePatientAppointment(appointmentEntity);
                 
                if (result.Success)
                {
                    return Ok(result);

                }
                return BadRequest(result.Message);
            }
            return NotFound(new Result { Success = false, Message = "Appointment not found." });
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

        [HttpPost("UpdateStatus")]
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

        [HttpGet("GetAppointmentByPatientId")]
        public async Task<ActionResult<IEnumerable<PatientAppointments>>> GetAppointmentsByPatientId(int patientId)
        {
            try
            {
                var appointments = await _patientAppointment.GetAppointmentByPatientId(patientId);
                if (appointments == null || appointments.Count == 0)
                {
                    return NotFound("No appointments found for the given patient ID.");
                }

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetAppointmentByDoctorId")]
        public async Task<ActionResult<IEnumerable<PatientAppointments>>> GetAppointmentsByDoctorId(int doctorId)
        {
            try
            {
                var appointments = await _patientAppointment.GetAppointmentByDoctorId(doctorId);
                if (appointments == null || appointments.Count == 0)
                {
                    return NotFound("No appointments found for the given Dcotor ID.");
                }

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
