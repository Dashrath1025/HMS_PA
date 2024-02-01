using System.ComponentModel.DataAnnotations;

namespace Hospital.Services.Clinic.Models
{
    public class Doctor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public string? Designation { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Fees must be a positive number")]
        public double? Fees { get; set; }

        public string? Address { get; set; }

        public string DoctorId { get; set; }
    }
}
