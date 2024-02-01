using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Hospital.Services.Clinic.Models
{
    public class PatientAppointments
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public int Did { get; set; }

        public string SerialNo { get; set; }

     //    [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Note { get; set; }

    }

}
