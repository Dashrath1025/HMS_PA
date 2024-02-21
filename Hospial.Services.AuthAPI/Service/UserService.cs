using Hospital.Services.AuthAPI.Data;
using Hospital.Services.AuthAPI.Models;
using Hospital.Services.AuthAPI.Models.Dto;
using Hospital.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.AuthAPI.Service
{
    public class UserService:IUserService
    {
        private readonly AppDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(AppDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;
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
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Role = role,
                    LockOutEnd = user.LockoutEnd
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
                return new Result { Message = "User not found" };
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
                await _db.SaveChangesAsync();
                return new Result { Success = true, Message = "unlocked" };
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddMonths(1);
                await _db.SaveChangesAsync();
                return new Result { Success = true, Message = "locked" };
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

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return null; // Return 404 if user is not found
            }

            return user; // Return user if found
        }

    }
}
