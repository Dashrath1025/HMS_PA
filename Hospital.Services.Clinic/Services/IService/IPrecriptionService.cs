using Hospital.Services.Clinic.Models;

namespace Hospital.Services.Clinic.Services.IService
{
    public interface IPrecriptionService
    {

        Task AddPrecription(Prescription prescription);
        Task<Result> UpdatePrescription(Prescription prescription);
        Task<Result> DeletePrescription(Guid prescriptionId);
    }
}
