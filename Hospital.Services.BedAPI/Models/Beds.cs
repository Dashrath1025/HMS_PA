using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Services.BedAPI.Models
{
    public class Beds
    {
        public int Id { get; set; }
        public int BedCatId { get; set; }

        [Required(ErrorMessage ="Bed No is Required")]
        public string No { get; set; }
        public string? Description {  get; set; }

        [ForeignKey("BedCatId")]
        public BedCategory BedCategory { get; set; }
    }
}
