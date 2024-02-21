namespace Hospital.Services.AuthAPI.Models.Dto
{
    public class UserRoleDTO
    { 
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset? LockOutEnd { get; set; }
        public string Role { get; set; }
    }
}
