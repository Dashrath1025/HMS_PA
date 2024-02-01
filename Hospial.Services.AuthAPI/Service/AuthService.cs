using Hospital.Services.AuthAPI.Data;
using Hospital.Services.AuthAPI.Models;
using Hospital.Services.AuthAPI.Models.Dto;
using Hospital.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _rolManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            if (user == null)
            {
                // User not found
                return new LoginResponseDto() { User = null, Token = "" };
            }

            var signInResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!signInResult)
            {
                // Invalid password
                return new LoginResponseDto() { User = null, Token = "" };
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                // User is locked out
                return new LoginResponseDto() { User = null, Token = "", Message = "You are Blocked can not Login, Please Contact Administrator" };
            }

            // If user was found and is allowed to sign in, generate JWT Token
            var roles= await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user,roles);

            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            };

            LoginResponseDto responseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };

            return responseDto;
        }


        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {


            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
                Gender=registrationRequestDto.Gender,
                DOB = registrationRequestDto.DOB,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };

            try
            {
                if(!await _rolManager.RoleExistsAsync(WC.Admin))
                {
                    await _rolManager.CreateAsync(new IdentityRole(WC.Admin));
                    await _rolManager.CreateAsync(new IdentityRole(WC.General));
                }

                var result = await _userManager.CreateAsync(user,registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        FirstName = userToReturn.FirstName,
                        LastName = userToReturn.LastName,
                        PhoneNumber = userToReturn.PhoneNumber,
                    };

                    int count = _userManager.Users.Count();

                    if (count == 1)
                    {
                        await _userManager.AddToRoleAsync(user, WC.Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, WC.General);

                    }

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
