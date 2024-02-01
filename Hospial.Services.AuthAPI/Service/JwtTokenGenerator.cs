using Hospital.Services.AuthAPI.Models;
using Hospital.Services.AuthAPI.Service.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hospital.Services.AuthAPI.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IHttpContextAccessor _contextAccessor;
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, IHttpContextAccessor contextAccessor)
        {
            _jwtOptions = jwtOptions.Value;
            _contextAccessor = contextAccessor;
        }
        public string GenerateToken(ApplicationUser applicationUser,IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, (applicationUser.FirstName.ToString() + " " + applicationUser.LastName))
            };

            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token= tokenHandler.CreateToken(tokenDescriptor);
            var tokenstring= tokenHandler.WriteToken(token);

            _contextAccessor.HttpContext.Response.Cookies.Append("token", tokenstring, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(7),
                Secure = true, // Set to true if your application uses HTTPS
                IsEssential=true,
                SameSite = SameSiteMode.None // Adjust as per your requirements
            }) ;

            return tokenstring;
        }
    }
}
