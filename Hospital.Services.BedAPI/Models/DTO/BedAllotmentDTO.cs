using System.ComponentModel.DataAnnotations;

namespace Hospital.Services.BedAPI.Models.DTO
{
    public class BedAllotmentDTO
    {
        [Required]
        public int Pid { get; set; }
        //[Required]
        //public int BedCatId { get; set; }
        [Required]
        public int BedId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AllotmentDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DischargeDate { get; set; }
        public string? Note { get; set; }

      //  public bool Released { get; set; } = false;
    }
}
