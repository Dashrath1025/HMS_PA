using AutoMapper;
using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Models.DTO;
using Hospital.Services.Clinic.Services.IService;
using Hospital.Services.ClinicAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Hospital.Services.Clinic.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
   
    public PatientController(IPatientService patientService, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _patientService = patientService;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet("GetPatients")]
    public async Task<IEnumerable<Patient>> GetPatientsAsync()
    {
        //var patients = await _doctorService.GetPatientsAsync();
        return await _patientService.GetPatientsAsync();
    }

    [HttpPost("Add")]
    public async Task<IActionResult> AddPatient([FromBody] PatientDTO patientDTO)
    {
        // Validate the model state
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

       

        var registerResult = await _patientService.RegisterService(patientDTO);

        if (!registerResult.StartsWith("Error"))
        {
            var patient = _mapper.Map<Patient>(patientDTO);

            var user = await _userManager.FindByEmailAsync(patientDTO.Email);
            await _userManager.RemoveFromRoleAsync(user, "General");
            await _userManager.AddToRoleAsync(user, "Patient");

            // Assign the patient ID to the local patient entity
            patient.PatientId = user.Id;

            // Add the patient to the local database
            await _patientService.AddPatientAsync(patient);

            return Ok("Patient Added Successfully!");
        }

       

        return BadRequest(ModelState);
    }


    [HttpPut("Update")]
    public async Task<IActionResult> UpdatePatient([FromBody] Patient patient)
    {

        var result = await _patientService.UpdatePatientAsync(patient);

        if (result.Success)
        {
            return Ok(new { Success = true, Message = result.Message });
        }

        else
        {
            return BadRequest(new { Success = false, Message = result.Message });
        }


    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> DeletePatient(int patientId)
    {
        var result = await _patientService.DeletePatientAsync(patientId);

        if (result.Success)
        {
            return Ok(new { Success = true, Message = result.Message });
        }
        else
        {
            return BadRequest(new { Success = false, Message = result.Message });
        }
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetPatientByIdAsync(int Id)
    {
        try
        {
            var patient = await _patientService.GetPatientByIdAsync(Id);

            if (patient != null)
            {
                return Ok(patient);
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

    public async Task<IActionResult> UpdateProfile(int pid, [FromBody] UpdatePatientProfileDTO updatePatientProfileDTO)
    {
        try
        {

            var result = await _patientService.UpdateProfile(pid, updatePatientProfileDTO);

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
