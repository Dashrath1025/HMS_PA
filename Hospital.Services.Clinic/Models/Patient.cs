using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Services.Clinic.Models
{
    public class Patient
    {
        [Key]
      //  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        public string? MaritalStatus { get; set; }

        public string? BloodGroup { get; set; }

        public string? Address { get; set; }

       // [Required(ErrorMessage = "PatientId is required")]
        public string PatientId { get; set; }
    }
}
