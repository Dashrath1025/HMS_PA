using Hospital.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.AuthAPI.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    }
}
