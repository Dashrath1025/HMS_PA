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

        [HttpGet("GetPrescription")]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptions()
        {
            var prescriptions = await _precriptionService.GetPrescription();
            return Ok(prescriptions);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<Prescription>> GetPrescriptionById(string prescriptionId)
        {
            var prescription = await _precriptionService.GetPrescriptionById(prescriptionId);

            if (prescription == null)
            {
                return NotFound(); // Returns 404 Not Found
            }

            return Ok(prescription); // Returns 200 OK with the prescription object
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

        public async Task<IActionResult> UpdatePrescription(Guid id, [FromBody] PrescriptionDTO prescriptionDTO)
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

        [HttpGet("GetByPatient")]
        public async Task<ActionResult<List<Prescription>>> GetPrescriptionsByPatient(string patientId)
        {
            try
            {
                var prescriptions = await _precriptionService.GetPrescriptionsByPatient(patientId);

                if (prescriptions == null || prescriptions.Count == 0)
                {
                    return NotFound(); // Return 404 Not Found if no prescriptions found for the patient
                }

                return Ok(prescriptions); // Return 200 OK with prescriptions
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 Internal Server Error for unexpected errors
            }
        }

        [HttpGet("GetByDoctor")]
        public async Task<ActionResult<List<Prescription>>> GetPrescriptionsByDoctor(string doctorId)
        {
            try
            {
                var prescriptions = await _precriptionService.GetPrescriptionsByDoctor(doctorId);

                if (prescriptions == null || prescriptions.Count == 0)
                {
                    return NotFound(); // Return 404 Not Found if no prescriptions found for the patient
                }

                return Ok(prescriptions); // Return 200 OK with prescriptions
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 Internal Server Error for unexpected errors
            }
        }


    }
}
