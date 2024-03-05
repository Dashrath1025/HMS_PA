using AutoMapper;
using Hospital.Services.BedAPI.Migrations;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Models.DTO;
using Hospital.Services.BedAPI.Services;
using Hospital.Services.BedAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hospital.Services.BedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedAllotmentController : ControllerBase
    {

        private readonly IBedAllotment _bedAllotment;
        private readonly IMapper _mapper;

        public BedAllotmentController(IBedAllotment bedAllotment, IMapper mapper)
        {
            _bedAllotment = bedAllotment;
            _mapper = mapper;
        }


       [HttpGet("GetBedAllotment")]
        public async Task<IEnumerable<BedAllotment>> GetAllBedAllotments()
        {
            return await _bedAllotment.GetAllBedAllotments();
        }

      // [Authorize]
        [HttpGet("GetBedAllotmentsWithPatients")]
        public async Task<IActionResult> GetBedAllotmentsWithPatients()
        {
            try
            {
                // Call the service method to get bed allotments with patients
                var bedAllotmentsWithPatients = await _bedAllotment.GetBedAllotmentsWithPatients();

                return Ok(bedAllotmentsWithPatients);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



        [HttpPost("Add")]

        public async Task<IActionResult> AddBedAllotment([FromBody] BedAllotmentDTO bedAllotmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bedAllotment = _mapper.Map<BedAllotment>(bedAllotmentDTO);

            bedAllotment.Released = false;

            var result = await _bedAllotment.AddBedAllotment(bedAllotment);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateBedAllotment(int id , [FromBody] BedAllotmentDTO bedAllotmentDTO,bool released)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Result { Success = false, Message = "Invalid data provided." });
            }

            var bedAllotment = _mapper.Map<BedAllotment>(bedAllotmentDTO);
            bedAllotment.Id = id;
            bedAllotment.Released=released;

            var result = await _bedAllotment.UpdateBedAllotment(bedAllotment);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteBedAllotment(int id)
        {
            var result = await _bedAllotment.DeleteBedAllotment(id);

            if (!result.Success)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }

        [HttpGet("GetById")]

        public async Task<IActionResult> GetBedById(int id)
        {
            try
            {
                // Call the service method to get bed allotments with patients
                var bedAllotmentsWithPatients = await _bedAllotment.GetBedAllotmentsWithPatientsById(id);

                return Ok(bedAllotmentsWithPatients);
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
