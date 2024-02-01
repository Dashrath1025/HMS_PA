using Hospital.Services.Clinic.Data;
using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Services.IService;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.Clinic.Services
{
    public class PatientAppointmentsService : IPatientAppointment
    {

        private readonly AppDbContext _db;

        public PatientAppointmentsService(AppDbContext db)
        {
            _db = db;

        }
        public async Task<Result> AddPatientAppointment(PatientAppointments patientAppointments)
        {
            await _db.PatientAppointments.AddAsync(patientAppointments);
            await _db.SaveChangesAsync();
            return new Result { Success = true,Message="Patient appointment added successfully" };
        }

        public async Task<Result> DeletePatientAppointment(int pId)
        {
            var appointmentToRemove = await _db.PatientAppointments.FindAsync(pId);
            if (appointmentToRemove != null)
            {
                _db.PatientAppointments.Remove(appointmentToRemove);
                await _db.SaveChangesAsync();
                return new Result { Success = true, Message = "Delete Success" };
            }

            return new Result { Success = false };
        }

        public async Task<List<PatientAppointments>> GetAppointmentAsync()
        {
            return await _db.PatientAppointments.ToListAsync();

        }

        public async Task<PatientAppointments> GetAppointmentIdAsync(int pId)
        {
            return await _db.PatientAppointments.FirstOrDefaultAsync(e => e.Id == pId);
        }

        public async Task<Result> UpdatePatientAppointment(PatientAppointments patientAppointments)
        {
            var existingAppointment = await _db.PatientAppointments.FindAsync(patientAppointments.Id);

            if (existingAppointment == null)
            {
                return new Result { Success = false, Message = "Appointment not found" };
            }

            _db.ChangeTracker.Clear();
            _db.PatientAppointments.Update(patientAppointments);
            await _db.SaveChangesAsync();
            return new Result { Success = true, Message = "Update Success" };

        }



        public bool IsPatientAlreadyScheduled(int patientId, DateTime appointmentDate)
        {
            return !_db.PatientAppointments.Any(a => a.Pid == patientId && a.AppointmentDate == appointmentDate);
        }

        public bool IsSerialNoAlreadyAssigned(int appointmentId, string serialNo)
        {
            return !_db.PatientAppointments.Any(a => a.SerialNo == serialNo && a.Id != appointmentId);
        }


    }
}
