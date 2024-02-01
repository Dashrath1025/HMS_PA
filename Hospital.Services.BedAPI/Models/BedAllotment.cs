using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Services.BedAPI.Models
{
    public class BedAllotment
    {
        public int Id { get; set; }
        public int Pid { get; set; }

      //  public int BedCatId { get; set; }
        public int BedId { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime AllotmentDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DischargeDate { get; set; }
        public bool Released { get; set; }
        public string? Note { get; set; }

       // [ForeignKey("Pid")]
       // public Patient patient { get; set; }

        //[ForeignKey("BedCatId")]
        //public BedCategory BedCategory { get; set; }

         [ForeignKey("BedId")]
        public Beds Beds { get; set; }

        [ForeignKey("Pid")]
        public Patient patient { get; set; }





    }
}
