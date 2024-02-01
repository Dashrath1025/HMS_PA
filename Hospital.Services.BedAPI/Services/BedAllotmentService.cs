using Hospital.Services.BedAPI.Data;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hospital.Services.BedAPI.Services
{
    public class BedAllotmentService : IBedAllotment
    {

        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BedAllotmentService(AppDbContext db, IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> AddBedAllotment(BedAllotment bedAllotment)
        {
            if (IsPatientAlreadyAllocated(bedAllotment.Pid))
            {
                return new Result { Success = false, Message = "Patient is already allocated a bed" };
            }

            if (IsBedAlreadyAllocated(bedAllotment.BedId, bedAllotment.AllotmentDate, bedAllotment.DischargeDate, 0))
            {
                return new Result { Success = false, Message = "Bed is already allocated for the selected period." };
            }

            await  _db.BedAllotments.AddAsync(bedAllotment);
            await _db.SaveChangesAsync();

            return new Result { Success = true, Message = "Bed Allotment added successfully." };
        }

        public async Task<Result> DeleteBedAllotment(int id)
        {
            var bedAllotmentToDelete = await _db.BedAllotments.FindAsync(id);

            if (bedAllotmentToDelete != null)
            {
                _db.BedAllotments.Remove(bedAllotmentToDelete);
                await _db.SaveChangesAsync();

                return new Result { Success = true, Message = "Delete Success" };
            }

            return new Result { Success = false, Message = "Bed Allotment Not Found" };
        }

        public async Task<List<BedAllotment>> GetAllBedAllotments()
        {
            return await _db.BedAllotments.Include(d=>d.Beds.BedCategory).ToListAsync();
        }

        public async Task<BedAllotment> GetBedAllotmentById(int id)
        {
            return await _db.BedAllotments
           //.Include(ba => ba.patient)
           //.Include(ba => ba.BedCategory)
           //.Include(ba => ba.Beds)
           .FirstOrDefaultAsync(ba => ba.Id == id);
        }

        public async Task<Result> UpdateBedAllotment(BedAllotment bedAllotment)
        {
            if (IsBedAlreadyAllocated(bedAllotment.BedId, bedAllotment.AllotmentDate, bedAllotment.DischargeDate, bedAllotment.Id))
            {
                return new Result { Success = false, Message = "Bed is already allocated for the selected period." };

            }

            var existingBedAllotment = await _db.BedAllotments.FindAsync(bedAllotment.Id);

            if (existingBedAllotment == null)
            {

                return new Result { Success = false, Message = "Bed Allotment Not Found" };

            }

            _db.ChangeTracker.Clear();
            _db.BedAllotments.Update(bedAllotment);
            await _db.SaveChangesAsync();

            return new Result { Success = true, Message = "Update Success" };

        }

        public bool IsBedAlreadyAllocated(int bedId, DateTime allotmentDate, DateTime dischargeDate, int currentBedAllotmentId)
        {
            // Check if there is any overlapping bed allotment for the selected bed and period
            return _db.BedAllotments.Any(ba =>
                ba.BedId == bedId &&
                ba.Id != currentBedAllotmentId &&
                ((allotmentDate >= ba.AllotmentDate && allotmentDate <= ba.DischargeDate) ||
                 (dischargeDate >= ba.AllotmentDate && dischargeDate <= ba.DischargeDate) ||
                 (allotmentDate <= ba.AllotmentDate && dischargeDate >= ba.DischargeDate)));
        }

        public bool IsPatientAlreadyAllocated(int patientId)
        {
            return _db.BedAllotments.Any(pa => pa.Pid == patientId);
        }

        public async Task<IEnumerable<BedAllotment>> GetBedAllotmentsWithPatients()
        {
            try
            {
                var bedAllotments = await GetAllBedAllotments();

                var result = new List<BedAllotment>();


                foreach (var bedAllotment in bedAllotments)
                {
                    var patient = await GetPatientDetails(bedAllotment.Pid);

                    bedAllotment.patient = patient;

                    result.Add(bedAllotment);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Bedallotment details", ex);
            }
        }

        public async Task<Patient> GetPatientDetails(int patientId)
         {
            try
            {


                var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["token"];

                if (string.IsNullOrEmpty(accessToken))
                {
                    return null;
                }

                var clinicApiUrl = _configuration.GetValue<string>("GetClinicAPI:url");

                var fullUrl = clinicApiUrl + $"api/Patient/GetById?Id={patientId}";

                HttpClient client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage response = await client.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize patient details
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var patient = JsonConvert.DeserializeObject<Patient>(responseBody);

                    return patient;
                }
                else
                {
                    // Handle the error
                    throw new Exception($"Error fetching patient details: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                throw new Exception("Error fetching patient details", ex);
            }
        }


    }
}
