using Hospital.Services.AuthAPI.Data;
using Hospital.Services.AuthAPI.Models;
using Hospital.Services.AuthAPI.Models.Dto;
using Hospital.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.AuthAPI.Service
{
    public class RoleService : IRoleService
    {

        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(AppDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;
        }



        public async Task<Result> CreateRoleAsync(string roleName)
        {
            if (await RoleExistAsync(roleName))
            {
                return new Result { Success = false, Message = "Role already exists" };
            }

            var result = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });

            return new Result
            {
                Success = result.Succeeded,
                Message = result.Succeeded ? "Role created successfully" : "Failed to create role"
            };
        }

        public async Task<Result> UpdateRoleAsync(RoleDTO roleDTO)
        {
            var roleToUpdate = await _db.Roles.FirstOrDefaultAsync(u => u.Id == roleDTO.Id);

            if (roleToUpdate == null)
            {
                return new Result { Success = false, Message = "Role not found" };
            }

            roleToUpdate.Name = roleDTO.Name;
            roleToUpdate.NormalizedName = roleDTO.Name.ToUpper();

            var result = await _roleManager.UpdateAsync(roleToUpdate);

            return new Result
            {
                Success = result.Succeeded,
                Message = result.Succeeded ? "Role updated successfully" : "Failed to update role"
            };
        }

        public async Task<Result> DeleteRoleAsync(string roleId)
        {
            var roleToDelete = await _db.Roles.FirstOrDefaultAsync(t => t.Id == roleId);

            if (roleToDelete == null)
            {
                return new Result { Success = false, Message = "Role not found" };
            }

            var userRoleAssignments = _db.UserRoles.Count(ur => ur.RoleId == roleId);

            if (userRoleAssignments > 0)
            {
                return new Result { Success = false, Message = "Cannot delete role, it is assigned to one or more users" };
            }

            _db.Roles.Remove(roleToDelete);
            await _db.SaveChangesAsync();

            return new Result { Success = true, Message = "Role deleted successfully" };
        }

        public async Task<bool> RoleExistAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

       
 
        public async Task<RoleDTO> GetRoleByid(string roleId)
        {
            var obj = await _db.Roles.FirstOrDefaultAsync(w => w.Id == roleId);

            if (obj == null)
            {
                return new RoleDTO { };
            }

            return new RoleDTO { Id=obj.Id,Name = obj.Name };
        }

        public async Task<RoleDTO> GetRoleByIdAsync(string roleId)
        {
            var role = await _db.Roles
                .Where(r => r.Id == roleId)
                .Select(r => new RoleDTO { Id = r.Id, Name = r.Name })
                .FirstOrDefaultAsync();

            return role;
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
        {
            var roles = await _db.Roles.Select(r => new RoleDTO { Id = r.Id, Name = r.Name }).ToListAsync();
            return roles;
        }
    }
}
