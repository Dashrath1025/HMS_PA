using Hospital.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("getuserRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userDTOList = await _userService.GetUsersWithRolesAsync();
            return Ok(userDTOList);
        }

        //[HttpGet("getrolename/{userId:guid}")]

        //public async Task<IActionResult> GetRoleNameByUserId([FromRoute] string userId)
        //{
        //    var roleName = await _userService.GetRoleNameByUserIdAsync(userId);
        //    return roleName != null ? Ok(roleName) : NotFound("Role not found for the user");

        //}

        //[HttpPost("assignrole")]

        //public async Task<IActionResult> AssignRole(string userEmail, string roleName)
        //{
        //    var result = await _userService.AssignRoleAsync(userEmail, roleName);

        //    if (result.Success)
        //    {
        //        return Ok(result.Message);
        //    }

        //    return BadRequest(result.Message);
        //}


        [HttpPost("lockunlock")]

        public async Task<IActionResult> LockUnlockUser(string userId)
        {
            var result = await _userService.LockUnlockUserAsync(userId);

            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }

            return NotFound(new { message = result.Message });
        }

        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(string Id)
        {
            var user = await _userService.GetUserById(Id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

    }
}
