using Hospital.Services.BedAPI.Models;

namespace Hospital.Services.BedAPI.Services.IServices
{
    public interface IBed
    {

        Task<List<Beds>> GetAllBeds();
        Task<Beds> GetBedById(int id);
        Task<Result> AddBed(Beds bed);
        Task<Result> UpdateBed(Beds bed);
        Task<Result> DeleteBed(int id);
    }

}

