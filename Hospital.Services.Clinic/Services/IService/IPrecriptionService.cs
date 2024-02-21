using Hospital.Services.Clinic.Models;

namespace Hospital.Services.Clinic.Services.IService
{
    public interface IPrecriptionService
    {
        Task<List<Prescription>> GetPrescription();
        Task AddPrecription(Prescription prescription);
        Task<Result> UpdatePrescription(Prescription prescription);
        Task<Result> DeletePrescription(Guid prescriptionId);
        Task<Prescription> GetPrescriptionById(string prescriptionId);
        Task<List<Prescription>> GetPrescriptionsByPatient(string patientId);
        Task<List<Prescription>> GetPrescriptionsByDoctor(string doctorId);
    }
}
