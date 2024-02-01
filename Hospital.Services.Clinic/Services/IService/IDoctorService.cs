using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Models.DTO;
using Hospital.Services.ClinicAPI.Models.DTO;

namespace Hospital.Services.Clinic.Services.IService
{
    public interface IDoctorService
    {
        Task<List<Doctor>> GetDoctorsAsync();
        Task AddDoctorAsync(Doctor doctor);
        Task<Result> UpdateDoctorAsync(Doctor doctor);
        Task<Result> DeleteDoctorAsync(int doctorId);
        Task<Doctor> GetDoctorByIdAsync(int doctorId);
        Task<string> RegisterService(PatientDTO patientDTO);
        Task<Result> UpdateProfile(int dId,UpdateDoctorProfileDTO updateDoctorProfileDTO);



    }
}


