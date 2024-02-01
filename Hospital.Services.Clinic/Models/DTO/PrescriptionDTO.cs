using System.ComponentModel.DataAnnotations;

namespace Hospital.Services.Clinic.Models.DTO
{
    public class PrescriptionDTO
    {

        [Required(ErrorMessage = "Appointment is required")]
        public int appointmentId { get; set; }

        [Required(ErrorMessage = "Symptoms is required")]
        public string Symptoms { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        public string Diagnosis { get; set; }
        public string? Advice { get; set; }

    }
}
