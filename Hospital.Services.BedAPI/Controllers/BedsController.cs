using AutoMapper;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Models.DTO;
using Hospital.Services.BedAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Services.BedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedsController : ControllerBase
    {
        private readonly IBed _bed;
        private readonly IMapper _mapper;

        public BedsController(IBed bed, IMapper mapper)
        {
            _bed = bed;
            _mapper = mapper;
        }

        [HttpGet("GetBeds")]
        public async Task<ActionResult<IEnumerable<Beds>>> GetAllBeds()
        {
            return await _bed.GetAllBeds();
        }

        [HttpPost("Add")]

        public async Task<IActionResult> AddBed(BedDTO bedDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var bed = _mapper.Map<Beds>(bedDTO);
            var result= await _bed.AddBed(bed);

            if (result.Success)
            {
                return Ok(result);

            }

            return BadRequest(result);


        }

        [HttpPut("Update")]

        public async Task<IActionResult> UpdateBed(int id, [FromBody] BedDTO bedDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var bed = _mapper.Map<Beds>(bedDTO);
                bed.Id = id;

               var result= await _bed.UpdateBed(bed);
               
                if(result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);

            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to Update {ex.Message}");
            }

        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<Result>> DeleteBed(int id)
        {
            var result = await _bed.DeleteBed(id);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }


    }
}
