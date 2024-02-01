using Microsoft.AspNetCore.Identity;

namespace Hospital.Services.AuthAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
    }
}
