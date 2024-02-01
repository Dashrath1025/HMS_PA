using System.ComponentModel.DataAnnotations;

namespace Hospital.Services.BedAPI.Models.DTO
{
    public class BedCategoryDTO
    {
     //   public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}
