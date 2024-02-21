using Hospital.Services.AuthAPI.Models;
using Hospital.Services.AuthAPI.Models.Dto;

namespace Hospital.Services.AuthAPI.Service.IService
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetRolesAsync();
        Task<bool> RoleExistAsync(string roleName);
        Task<Result> CreateRoleAsync(string roleName);
        Task<Result> UpdateRoleAsync(RoleDTO roleDTO);
        Task<Result> DeleteRoleAsync(string roleId);
        Task<RoleDTO> GetRoleByid(string roleId);
    }

    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
