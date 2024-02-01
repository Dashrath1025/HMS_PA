using Hospital.Services.AuthAPI.Models.Dto;
using Hospital.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("getrole")]

        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();
            return Ok(roles);
        }

        [HttpPost("Add")]

        public async Task<IActionResult> AddRole(RoleDTO roleDTO)
        {
            var result = await _roleService.CreateRoleAsync(roleDTO.Name);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);

        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateRole(RoleDTO roleDTO)
        {
            var result = await _roleService.UpdateRoleAsync(roleDTO);

            if (result == null)
            {
                return NotFound("Role Not Found");
            }

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleService.DeleteRoleAsync(id);

            if (result == null)
            {
                return NotFound("Role Not Found");
            }
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }


        [HttpGet("getuserRoles")]

        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userDTOList = await _roleService.GetUsersWithRolesAsync();
            return Ok(userDTOList);
        }

        [HttpGet("getrolename/{userId:guid}")]

        public async Task<IActionResult> GetRoleNameByUserId([FromRoute] string userId)
        {
            var roleName = await _roleService.GetRoleNameByUserIdAsync(userId);
            return roleName != null ? Ok(roleName) : NotFound("Role not found for the user");

        }

        [HttpPost("assignrole")]

        public async Task<IActionResult> AssignRole(string userEmail, string roleName)
        {
            var result = await _roleService.AssignRoleAsync(userEmail, roleName);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }


        [HttpPost("lockunlock")]

        public async Task<IActionResult> LockUnlockUser(string userId)
        {
            var result = await _roleService.LockUnlockUserAsync(userId);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return NotFound(result.Message);
        }

        [HttpGet("GetById/{id:guid}")]

        public async Task<IActionResult> GetRole([FromRoute] string id)
        {
            var result = await _roleService.GetRoleByid(id);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
