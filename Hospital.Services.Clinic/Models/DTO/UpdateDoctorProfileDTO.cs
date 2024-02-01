using System.ComponentModel.DataAnnotations;

namespace Hospital.Services.ClinicAPI.Models.DTO
{
    public class UpdateDoctorProfileDTO
    {
        public string Designation { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Fees must be a positive number")]
        public double? Fees { get; set; }

        public string Address { get; set; }
    }
}
