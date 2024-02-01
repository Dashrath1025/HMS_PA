using System.ComponentModel.DataAnnotations;

namespace Hospital.Services.BedAPI.Models
{
    public class BedCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Bed Category name is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
