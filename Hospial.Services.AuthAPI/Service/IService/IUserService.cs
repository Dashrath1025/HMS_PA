using Hospital.Services.AuthAPI.Models;
using Hospital.Services.AuthAPI.Models.Dto;

namespace Hospital.Services.AuthAPI.Service.IService
{
    public interface IUserService
    {
        Task<IEnumerable<UserRoleDTO>> GetUsersWithRolesAsync();
        Task<Result> GetRoleNameByUserIdAsync(string userId);
        Task<Result> AssignRoleAsync(string userEmail, string roleName);
        Task<Result> LockUnlockUserAsync(string userId);
        Task<ApplicationUser> GetUserById(string Id);
    }
}
