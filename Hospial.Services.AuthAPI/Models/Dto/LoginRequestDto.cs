using System.ComponentModel.DataAnnotations;

namespace Hospital.Services.AuthAPI.Models.Dto
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        [EmailAddress(ErrorMessage = "Invalid User Name.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }
}
