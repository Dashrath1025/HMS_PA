using Hospital.Services.Clinic.Data;
using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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
            if (IsPatientAlreadyScheduled(patientAppointments.Pid, 0))
            {
                if (IsAppointmentAlreadyScheduled(patientAppointments.AppointmentDate, 0,patientAppointments.StartTime,patientAppointments.EndTime,patientAppointments.Did))
                {
                    if (IsSerialNoAlreadyAssigned(0, patientAppointments.SerialNo))
                    {
                        await _db.PatientAppointments.AddAsync(patientAppointments);
                        await _db.SaveChangesAsync();
                        return new Result { Success = true, Message = "Patient appointment added successfully" };
                    }
                    return new Result { Success = false, Message = "Serial No is already assigned to another appointment." };

                }
                return new Result { Success = false, Message = "Selected Date is Alreday Appointment Scheduled, Please Select another Date & Time" };

            }
            return new Result { Success = false, Message = "This Patient already has an Scheduled Appointment" };
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
            return await _db.PatientAppointments.Include(t=>t.Patient).Include(t => t.Doctor).ToListAsync();

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
            if (IsPatientAlreadyScheduled(patientAppointments.Pid, patientAppointments.Id))
            {
                if (IsAppointmentAlreadyScheduled(patientAppointments.AppointmentDate, patientAppointments.Id,patientAppointments.StartTime,patientAppointments.EndTime, patientAppointments.Did))
                {
                    if (IsSerialNoAlreadyAssigned(patientAppointments.Id, patientAppointments.SerialNo))
                    {
                        _db.ChangeTracker.Clear();
                        _db.PatientAppointments.Update(patientAppointments);
                        await _db.SaveChangesAsync();
                        return new Result { Success = true, Message = "Appointment Update Success" };
                    }
                    return new Result { Success = false, Message = "Serial No is already assigned to another appointment." };

                }
                return new Result { Success = false, Message = "Selected Date & Time is Alreday Appointment Scheduled, Please Select another Date & Time" };
            }
            return new Result { Success = false, Message = "This Patient already has an Scheduled Appointment" };
        }


        public bool IsPatientAlreadyScheduled(int patientId, int appointmentId)
        {
            return !_db.PatientAppointments.Any(a => a.Pid == patientId && a.Id != appointmentId);
        }

        public bool IsAppointmentAlreadyScheduled(DateTime appointmentDate, int Id,DateTime StartTime,DateTime EndTime,int Did)
        {
            bool isScheduled = _db.PatientAppointments.Any(a => a.Id != Id &&
                                                                a.Did == Did && // Check for the same doctor ID
                                                                a.AppointmentDate.Date == appointmentDate.Date &&
                                                                (
                                                                    // Check if the start time falls within the range of existing appointments
                                                                    (StartTime >= a.StartTime && StartTime < a.EndTime) ||
                                                                    // Check if the end time falls within the range of existing appointments
                                                                    (EndTime > a.StartTime && EndTime <= a.EndTime) ||
                                                                    // Check if the existing appointment falls within the range of new appointment
                                                                    (StartTime <= a.StartTime && EndTime >= a.EndTime)
                                                                ));

            return !isScheduled;
        }


        public bool IsSerialNoAlreadyAssigned(int appointmentId, string serialNo)
        {
            return !_db.PatientAppointments.Any(a => a.SerialNo == serialNo && a.Id != appointmentId);
        }

        public async Task<List<PatientAppointments>> GetAppointmentByPatientId(int Id)
        {
            return await _db.PatientAppointments.Include(d=>d.Doctor)
                .Where(t => t.Pid == Id)
                .ToListAsync();
        }

        public async Task<List<PatientAppointments>> GetAppointmentByDoctorId(int Id)
        {
            return await _db.PatientAppointments.Include(d => d.Patient)
                 .Where(t => t.Did == Id)
                 .ToListAsync();
        }
    }
}
