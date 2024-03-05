using AutoMapper;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Models.DTO;
using Hospital.Services.BedAPI.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Services.BedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedCategoryController : ControllerBase
    {
        private readonly IBedCategory _bedCategory;
        private readonly IMapper _mapper;

        public BedCategoryController(IBedCategory bedCategory, IMapper mapper)
        {
            _bedCategory = bedCategory;
            _mapper = mapper;
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet("GetBedCategory")]

        public async Task<IEnumerable<BedCategory>> BedCategories()
        {
            return await _bedCategory.GetAllBedCategories();
        }

        [HttpPost("Add")]

        public async Task<IActionResult> AddBedCategory(BedCategoryDTO bedCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var bedCategory = _mapper.Map<BedCategory>(bedCategoryDTO);
                var result = await _bedCategory.AddBedCategory(bedCategory);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);


            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to add {ex.Message}");
            }

        }

        [HttpPut("Update")]

        public async Task<IActionResult> UpdateBedCategory(int id, [FromBody] BedCategoryDTO bedCategoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var bedCat = _mapper.Map<BedCategory>(bedCategoryDTO);
                bedCat.Id = id;

                var result = await _bedCategory.UpdateBedCategory(bedCat);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);


            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update {ex.Message}");
            }
        }

        [HttpDelete("Delete")]

        public async Task<IActionResult> DeleteBedCategory(int id)
        {
            var result = await _bedCategory.DeleteBedCategory(id);

            if (result.Success)
            {
                return Ok(result);
            }

            else
            {
                return NotFound(result.Message);
            }
        }

        [HttpGet("GetById")]

        public async Task<IActionResult> GetBedCategory(int id)
        {
            var bedcat = await _bedCategory.GetBedCategoryById(id);

            if (bedcat == null)
            {
                return NotFound();
            }

            return Ok(bedcat);
        }

    }
}
