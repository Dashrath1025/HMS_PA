using Hospital.Services.BedAPI.Models;

namespace Hospital.Services.BedAPI.Services.IServices
{
    public interface IBedAllotment
    {
        Task<List<BedAllotment>> GetAllBedAllotments();
        Task<BedAllotment> GetBedAllotmentById(int id);
        Task<Result> AddBedAllotment(BedAllotment bedAllotment);
        Task<Result> UpdateBedAllotment(BedAllotment bedAllotment);
        Task<Result> DeleteBedAllotment(int id);

        bool IsBedAlreadyAllocated(int bedId, DateTime allotmentDate, DateTime dischargeDate, int currentBedAllotmentId);

        bool IsPatientAlreadyAllocated(int patientId);

        Task<IEnumerable<BedAllotment>> GetBedAllotmentsWithPatients();
        Task<Patient> GetPatientDetails(int patientId);
    }
    public class BedAllotmentWithPatientDetails
    {
        public BedAllotment BedAllotment { get; set; }
        public Patient PatientDetails { get; set; }
    }

}
