using Hospital.Services.BedAPI.Models.DTO;
using Hospital.Services.BedAPI.Models;

namespace Hospital.Services.BedAPI.Services.IServices
{
    public interface IBedCategory
    {
        Task<List<BedCategory>> GetAllBedCategories();
        Task<BedCategory> GetBedCategoryById(int id);
        Task<Result> AddBedCategory(BedCategory bedCategory);
        Task<Result> UpdateBedCategory(BedCategory bedCategory);
        Task<Result> DeleteBedCategory(int id);
    }

    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

}
