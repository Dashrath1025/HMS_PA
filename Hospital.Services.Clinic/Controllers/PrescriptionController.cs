using AutoMapper;
using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Models.DTO;
using Hospital.Services.Clinic.Services;
using Hospital.Services.Clinic.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Services.Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrecriptionService _precriptionService;
        private readonly IPatientAppointment _patientAppointment;
        private readonly IMapper _mapper;
        public PrescriptionController(IPrecriptionService precriptionService, IPatientAppointment patientAppointment, IMapper mapper)
        {
            _precriptionService = precriptionService;
            _patientAppointment = patientAppointment;
            _mapper = mapper;
        }

        [HttpPost("Add")]

        public async Task<IActionResult> AddPrescription([FromBody] PrescriptionDTO prescriptionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var appointment = await _patientAppointment.GetAppointmentIdAsync(prescriptionDTO.appointmentId);

                if (appointment == null)
                {
                    return NotFound(new { Message = "Appointment not found." });
                }

                var prescription = _mapper.Map<Prescription>(prescriptionDTO);

                prescription.CheckupDate = appointment.AppointmentDate;

                await _precriptionService.AddPrecription(prescription);

                return Ok(new { Message = "Prescription added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }


        [HttpPut("Update")]

        public async Task<IActionResult> UpdatePrescription(Guid id,[FromBody] PrescriptionDTO prescriptionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var prescription = _mapper.Map<Prescription>(prescriptionDTO);
                prescription.Id = id;
                var result = await _precriptionService.UpdatePrescription(prescription);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("Delete")]

        public async Task<IActionResult> DeletePrescription(Guid id)
        {
            var result = await _precriptionService.DeletePrescription(id);

            if (result.Success)
            {
                return Ok(result);
            }

            return NotFound(result);
        }
    }
}
