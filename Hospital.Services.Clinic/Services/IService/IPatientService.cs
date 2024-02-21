using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Models.DTO;
using Hospital.Services.ClinicAPI.Models.DTO;

namespace Hospital.Services.Clinic.Services.IService
{
    public interface IPatientService
    {

        Task<List<Patient>> GetPatientsAsync();
        Task AddPatientAsync(Patient patient);
        Task<Result> UpdatePatientAsync(Patient patient);
        Task<Result> DeletePatientAsync(int patientId);

        Task<string> RegisterService(PatientDTO patientDTO);
        Task<Patient> GetPatientByIdAsync(int patientId);

        Task<Patient> GetByPatient(string pId);
        Task<Result> UpdateProfile(int pId, UpdatePatientProfileDTO updatePatientProfileDTO);

    }

    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }


}
