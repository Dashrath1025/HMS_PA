

using Hospital.Services.BedAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.BedAPI.Data
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

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
        }



    }
}
