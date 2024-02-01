using Hospital.Services.BedAPI.Data;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.BedAPI.Services
{
    public class BedService : IBed
    {
        private readonly AppDbContext _db;

        public BedService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Beds>> GetAllBeds()
        {
            return await _db.Beds.Include(b => b.BedCategory).ToListAsync();
        }

        public async Task<Beds> GetBedById(int id)
        {
            return await _db.Beds.Include(b => b.BedCategory).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Result> AddBed(Beds bed)
        {

            if (_db.Beds.Any(b => b.No == bed.No))
            {
                return new Result { Success = false, Message = "Bed No must be unique." };
            }


            if (!_db.BedCategories.Any(bc => bc.Id == bed.BedCatId))
            {
                return new Result { Success = false, Message = "Invalid BedCatId. Bed Category does not exist." };
            }

            _db.Beds.Add(bed);
            await _db.SaveChangesAsync();

            return new Result { Success = true, Message = "Bed added successfully." };
        }


        public async Task<Result> UpdateBed(Beds bed)
        {
            // Check if the bed No is unique
            if (_db.Beds.Any(b => b.Id != bed.Id && b.No == bed.No))
            {
                return new Result { Success = false, Message = "Bed No must be unique." };
            }

            // Check if the associated BedCatId exists
            if (!_db.BedCategories.Any(bc => bc.Id == bed.BedCatId))
            {
                return new Result { Success = false, Message = "Invalid BedCatId. Bed Category does not exist." };
            }

            var existingBed = await _db.Beds.FindAsync(bed.Id);

            if (existingBed == null)
            {
                return new Result { Success = false, Message = "Bed Not Found" };
            }

            _db.ChangeTracker.Clear();
            _db.Beds.Update(bed);
            await _db.SaveChangesAsync();
            return new Result { Success = true, Message = "Update Success" };
        }

        public async Task<Result> DeleteBed(int id)
        {
            var bedToDelete = await _db.Beds.FindAsync(id);

            if (bedToDelete != null)
            {
                _db.Beds.Remove(bedToDelete);
                await _db.SaveChangesAsync();
                return new Result { Success = true, Message = "Delete Success" };
            }

            return new Result { Success = false, Message = "Bed Not Found" };
        }
    }
}
