using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Services.Clinic.Models
{
    public class PatientAppointments
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public int Did { get; set; }
        public string SerialNo { get; set; }

     //    [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Note { get; set; }

        [ForeignKey("Pid")]
        public Patient Patient { get; set; }

        [ForeignKey("Did")]

        public Doctor Doctor { get; set; }

    }

}
