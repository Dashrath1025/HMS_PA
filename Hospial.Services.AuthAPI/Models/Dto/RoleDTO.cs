using System.ComponentModel.DataAnnotations;

namespace Hospital.Services.AuthAPI.Models.Dto
{
    public class RoleDTO
    {
        public string Id { get; set; }

        [Required(ErrorMessage ="Role Name Is required")]
        public string Name { get; set; }
    }
}
