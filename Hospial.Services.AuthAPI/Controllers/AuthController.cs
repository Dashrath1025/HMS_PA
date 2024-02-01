using Hospital.Services.AuthAPI.Models.Dto;
using Hospital.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

       private readonly IAuthService _authService;

        protected ResponseDto response;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            response = new();
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var errorMsg = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMsg))
            {
                response.IsSuccess = false;
                response.Message = errorMsg;
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var loginResponse = await _authService.Login(model);

            if (!string.IsNullOrEmpty(loginResponse.Message))
            {

                response.IsSuccess = false;
                response.Message = loginResponse.Message;
                return StatusCode(StatusCodes.Status403Forbidden, response);
            }

            if (loginResponse.User == null)
            {
                response.IsSuccess = false;
                response.Message = "Username or password is incorrect";
                return BadRequest(response);
            }

            response.Result = loginResponse;

            return Ok(response);
        }


    }
}
