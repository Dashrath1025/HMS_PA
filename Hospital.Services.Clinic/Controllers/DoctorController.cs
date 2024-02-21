using AutoMapper;
using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Models.DTO;
using Hospital.Services.Clinic.Services;
using Hospital.Services.Clinic.Services.IService;
using Hospital.Services.ClinicAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Services.Clinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public DoctorController(IDoctorService doctorService, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _doctorService = doctorService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("GetDoctors")]
        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
            return await _doctorService.GetDoctorsAsync();
        }

            [HttpPost("Add")]
        public async Task<IActionResult> AddDoctor([FromBody] PatientDTO patientDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResult = await _doctorService.RegisterService(patientDTO);


            if (!registerResult.StartsWith("Error"))
            {
                var doctor = _mapper.Map<Doctor>(patientDTO);

                var user = await _userManager.FindByEmailAsync(patientDTO.Email);
                await _userManager.RemoveFromRoleAsync(user, "General");
                await _userManager.AddToRoleAsync(user, "Doctor");

                doctor.DoctorId = user.Id;

                await _doctorService.AddDoctorAsync(doctor);

                return Ok("Doctor Added Successfully");
            }
            return BadRequest(new Result { Success = false, Message = "Something went wrong!" });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateDoctor([FromBody] Doctor doctor)
        {

            var result = await _doctorService.UpdateDoctorAsync(doctor);

            if (result.Success)
            {
                return Ok(result);
            }

            else
            {
                return BadRequest(result);
            }


        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteDoctor(int doctorId)
        {
            var result = await _doctorService.DeleteDoctorAsync(doctorId);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetDoctorByIdAsync(int Id)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(Id);

                if (doctor != null)
                {
                    return Ok(doctor);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpGet("GetDoctorProfile")]

        public async Task<IActionResult> GetDoctorById(string Id)
        {
            try
            {
                var doctor = await _doctorService.GetByDoctor(Id);

                if (doctor != null)
                {
                    return Ok(doctor);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }


        [HttpPut("UpdateProfile")]

        public async Task<IActionResult> UpdateProfile(int dId, [FromBody] UpdateDoctorProfileDTO updateDoctorProfileDTO)
        {
            try
            {

                var result = await _doctorService.UpdateProfile(dId, updateDoctorProfileDTO);

                if (result.Success)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Internal Server Error" });

            }
        }

    }
}
