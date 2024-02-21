using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hospital.Services.Clinic.Models.DTO
{
    public class PatientAppointmentsDto
    {
     //   public int Id { get; set; }
        public int Pid { get; set; }
        public int Did { get; set; }
        public string SerialNo { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        //  [DataType(DataType.DateTime)]   
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Note { get; set; }
    }

}
