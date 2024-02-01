using System.ComponentModel.DataAnnotations;

namespace Hospital.Services.BedAPI.Models.DTO
{
    public class BedDTO
    {
       
            [Required(ErrorMessage = "Bed No is Required")]
            public string No { get; set; }

            public string? Description { get; set; }

            public int BedCatId { get; set; }
        

    }
}
