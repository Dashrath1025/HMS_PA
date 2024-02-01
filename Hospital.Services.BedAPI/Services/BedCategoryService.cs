using Hospital.Services.BedAPI.Data;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Models.DTO;
using Hospital.Services.BedAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Services.BedAPI.Services
{
    public class BedCategoryService : IBedCategory

    {
        private readonly AppDbContext _db;

        public BedCategoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Result> AddBedCategory(BedCategory bedCategory)
        {
            // Check if the BedCategory Name is unique
            if (_db.BedCategories.Any(bc => bc.Name.ToLower() == bedCategory.Name.ToLower()))
            {
                return new Result { Success = false, Message = "Bed category with the same name already exists." };
            }

            await _db.BedCategories.AddAsync(bedCategory);
            await _db.SaveChangesAsync();

            return new Result { Success = true, Message = "Bed category added successfully." };
        }

        public async Task<Result> DeleteBedCategory(int id)
        {
            var bedCategoryToDelete = await _db.BedCategories.FindAsync(id);

            if (bedCategoryToDelete != null)
            {
                var associatedBeds = await _db.Beds.Where(b => b.BedCatId == id).ToListAsync();

                _db.Beds.RemoveRange(associatedBeds);

                _db.BedCategories.Remove(bedCategoryToDelete);

                await _db.SaveChangesAsync();

                return new Result { Success = true, Message = "Delete success" };
            }

            return new Result { Success = false, Message = "Not found" };
        }


        public async Task<List<BedCategory>> GetAllBedCategories()
        {
            return await _db.BedCategories.ToListAsync();
        }

        public async Task<BedCategory> GetBedCategoryById(int id)
        {
            return await _db.BedCategories.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Result> UpdateBedCategory(BedCategory bedCategory)
        {
            if (_db.BedCategories.Any(bc => bc.Name.ToLower() == bedCategory.Name.ToLower() && bc.Id != bedCategory.Id)) 
            {
                return new Result { Success = false, Message = "Bed category with the same name already exists." };
            }

            var existingBedCategory = await _db.BedCategories.FindAsync(bedCategory.Id);

            if (existingBedCategory == null)
            {
                return new Result { Success = false, Message = "Bed Category Not Found" };
            }

            _db.ChangeTracker.Clear();
            _db.BedCategories.Update(bedCategory);
            await _db.SaveChangesAsync();
            return new Result { Success = true, Message = "Update Success" };


        }

    }
}
