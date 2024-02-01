using Hospital.Services.AuthAPI.Data;
using Hospital.Services.AuthAPI.Models;
using Hospital.Services.AuthAPI.Models.Dto;
using Hospital.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
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

        public async Task<IEnumerable<UserRoleDTO>> GetUsersWithRolesAsync()
        {
            var userlist = await _db.ApplicationUsers.ToListAsync();
            var userRoleList = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            var userDTOList = userlist.Select(user =>
            {
                var userRole = userRoleList.FirstOrDefault(u => u.UserId == user.Id);
                var role = userRole == null ? "None" : roles.FirstOrDefault(u => u.Id == userRole.RoleId)?.Name ?? "None";

                return new UserRoleDTO
                {
                    Id = user.Id,
                    UserName = user.Email,
                    Email = user.Email,
                    Role = role
                };
            });

            return userDTOList;
        }

        public async Task<Result> GetRoleNameByUserIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new Result { Success = false, Message = "User Nor found" };
                }

                var roles = await _userManager.GetRolesAsync(user);

                if (roles != null && roles.Any())
                {
                    var roleName = await _roleManager.FindByNameAsync(roles[0]);

                    if (roleName != null)
                    {
                        return new Result { Success = true, Message = roleName.Name };
                    }
                }

                return new Result { Success = false, Message = "Role not found for the user" };
            }
            catch (Exception ex)
            {
                return new Result { Success = false, Message = ex.Message };
            }
        }


        public async Task<Result> LockUnlockUserAsync(string userId)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(e => e.Id == userId);

            if (objFromDb == null)
            {
                return new Result {  Message = "User not found" };
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
                await _db.SaveChangesAsync();
                return new Result { Message = "unlocked" };
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddMonths(1);
                await _db.SaveChangesAsync();
                return new Result { Message = "locked" };
            }
        }


        public async Task<Result> AssignRoleAsync(string userEmail, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return new Result { Success = false, Message = "User not Found" };

            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var existingRole = await _roleManager.FindByNameAsync(roleName);

            if (existingRole == null)
            {
                return new Result { Success = false, Message = "Role Not Found" };
            }

            foreach (var role in userRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);

            return result.Succeeded ?
                new Result { Success = true, Message = "User assigned Succefully" } :
                new Result { Success = false, Message = "Failed to assign role" };
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
    }
}
