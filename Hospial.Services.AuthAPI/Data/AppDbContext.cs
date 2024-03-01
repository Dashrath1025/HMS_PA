using Hospital.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Hospital.Services.AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // Seed dummy data
        //  //  SeedData(modelBuilder);
        //}

        //private void SeedData(ModelBuilder modelBuilder)
        //{
        //    // Add dummy users
        //    modelBuilder.Entity<ApplicationUser>().HasData(
        //        new ApplicationUser
        //        {
        //            Id = "1",
        //            UserName = "user1@example.com",
        //            NormalizedUserName = "USER1@EXAMPLE.COM",
        //            Email = "user1@example.com",
        //            NormalizedEmail = "USER1@EXAMPLE.COM",
        //            EmailConfirmed = true,
        //            PasswordHash = "AQAAAAEAACcQAAAAEBuRVdz6BbXTcldQ6DGVkssw9IK2i3RzZYLlA4ClPWnD5MOb/3+qzOUmu2VQMD0V8Q==", // Password is: Password@123
        //            SecurityStamp = "L46TXXPZKMSJ6P5AJVXRM4A22LIX6W2C",
        //            ConcurrencyStamp = "5a835e24-5465-42be-b607-6899b8ba926f"
        //        },
        //        new ApplicationUser
        //        {
        //            Id = "2",
        //            UserName = "user2@example.com",
        //            NormalizedUserName = "USER2@EXAMPLE.COM",
        //            Email = "user2@example.com",
        //            NormalizedEmail = "USER2@EXAMPLE.COM",
        //            EmailConfirmed = true,
        //            PasswordHash = "AQAAAAEAACcQAAAAEBuRVdz6BbXTcldQ6DGVkssw9IK2i3RzZYLlA4ClPWnD5MOb/3+qzOUmu2VQMD0V8Q==", // Password is: Password@123
        //            SecurityStamp = "L46TXXPZKMSJ6P5AJVXRM4A22LIX6W2C",
        //            ConcurrencyStamp = "5a835e24-5465-42be-b607-6899b8ba926f"
        //        }
        //    );
        //}
    }
}
