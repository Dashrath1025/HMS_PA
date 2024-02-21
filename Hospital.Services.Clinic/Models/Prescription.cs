using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Services.Clinic.Models
{
    public class Prescription
    {
        public Guid Id { get; set; }
        // public string DoctorId { get; set; }

        [Required(ErrorMessage = "Appointment is required")]
        public int appointmentId { get; set; }

        [Required(ErrorMessage = "Symptoms is required")]
        public string Symptoms { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        public string Diagnosis { get; set; }
        public string? Advice { get; set; }

        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CheckupDate { get; set; }

        [ForeignKey("appointmentId")]

        public PatientAppointments PatientAppointments { get; set; }

    }
}
