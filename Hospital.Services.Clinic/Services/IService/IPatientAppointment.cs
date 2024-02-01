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

        bool IsPatientAlreadyScheduled(int patientId, DateTime appointmentDate);
        bool IsSerialNoAlreadyAssigned(int appointmentId,string serialNo);
    }
}
