using Hospital.Services.Clinic.Models;

namespace Hospital.Services.Clinic.Services.IService
{
    public interface IPatientAppointment
    {
        Task<List<PatientAppointments>> GetAppointmentAsync();

        Task<Result> AddPatientAppointment(PatientAppointments patientAppointments);

        Task<Result> UpdatePatientAppointment(PatientAppointments patientAppointments);

        Task<Result> DeletePatientAppointment(int pId);

        Task<PatientAppointments> GetAppointmentIdAsync(int pId);
        Task<List<PatientAppointments>> GetAppointmentByPatientId(int Id);
        Task<List<PatientAppointments>> GetAppointmentByDoctorId(int Id);

    }
}
