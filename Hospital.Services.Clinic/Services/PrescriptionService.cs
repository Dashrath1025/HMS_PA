using Hospital.Services.Clinic.Data;
using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Services.IService;

namespace Hospital.Services.Clinic.Services
{
    public class PrescriptionService : IPrecriptionService
    {
        private readonly AppDbContext _db;

        public PrescriptionService(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddPrecription(Prescription prescription)
        {
             prescription.Id = Guid.NewGuid();
            _db.Prescriptions.Add(prescription);
            await _db.SaveChangesAsync();
        }

        public async Task<Result> DeletePrescription(Guid prescriptionId)
        {
            var prescriptionToDelete = await _db.Prescriptions.FindAsync(prescriptionId);

            if (prescriptionToDelete != null)
            {
                _db.Prescriptions.Remove(prescriptionToDelete);
                await _db.SaveChangesAsync();
                return new Result { Success = true, Message = "Delete prescription" };
            }

            return new Result { Success = false, Message = "Prescription Not Found" };
        }

        public async Task<Result> UpdatePrescription(Prescription prescription)
        {
            var existingPrescription =  _db.Prescriptions.FirstOrDefault(t=>t.Id==prescription.Id);
            prescription.CheckupDate = existingPrescription.CheckupDate;
            if (existingPrescription == null)
            {
                return new Result { Success = false, Message = "Not Found" };
            }

            _db.ChangeTracker.Clear();
            _db.Update(prescription);
            await _db.SaveChangesAsync();
            return new Result { Success = true, Message = "Update prescription" };

        }
    }
}
