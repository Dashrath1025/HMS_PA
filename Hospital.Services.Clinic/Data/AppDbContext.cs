

using Hospital.Services.Clinic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.Clinic.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<PatientAppointments> PatientAppointments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
    }
}
