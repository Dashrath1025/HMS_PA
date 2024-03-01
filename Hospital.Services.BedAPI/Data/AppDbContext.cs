using Hospital.Services.BedAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hospital.Services.BedAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


        public DbSet<BedCategory> BedCategories { get; set; }
        public DbSet<Beds> Beds { get; set; }
        public DbSet<BedAllotment> BedAllotments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure other relationships
            modelBuilder.Entity<BedAllotment>()
                .HasOne(ba => ba.Beds)
                .WithMany()
                .HasForeignKey(ba => ba.BedId)
                .OnDelete(DeleteBehavior.Restrict); // Set OnDelete to Restrict

            // Seed BedCategory data
            modelBuilder.Entity<BedCategory>().HasData(
                new BedCategory
                {
                    Id = 1,
                    Name = "General Ward",
                    Description = "General ward beds"
                },
                new BedCategory
                {
                    Id = 2,
                    Name = "Private Ward",
                    Description = "Private ward beds"
                }
                // Add more seed data as needed
            );
        }
    }
}
