using Hospital.Services.Clinic.Data;
using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Models.DTO;
using Hospital.Services.Clinic.Services.IService;
using Hospital.Services.ClinicAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Hospital.Services.Clinic.Services
{
    public class PatientService : IPatientService
    {

        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PatientService(AppDbContext db, UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task AddPatientAsync(Patient patient)
        {
            await _db.Patients.AddAsync(patient);
            await _db.SaveChangesAsync();
        }

        public async Task<string> RegisterService(PatientDTO patientDTO)
        {

            var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            var authUrl = _configuration.GetValue<string>("GetAuthAPI:url");

            var fullUrl = authUrl + $"api/Auth/register";


            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var content = new StringContent(JsonConvert.SerializeObject(patientDTO), Encoding.UTF8, "application/json");

                //send post request 
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(fullUrl, content);

                //check url

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    return responseContent;
                }
                else
                {
                    // Handle the error
                    return $"Error: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}";
                }
            }
        }

        public async Task<Result> DeletePatientAsync(int patientId)
        {
            var patient = await _db.Patients.FindAsync(patientId);

            if (patient == null)
            {
                return new Result { Success = false, Message = "Patient not found" };
            }

            var user = await _userManager.FindByIdAsync(patient.PatientId);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return new Result { Success = false, Message = "Failed to delete corresponding ApplicationUser" };
                }
            }

            _db.Patients.Remove(patient);
            await _db.SaveChangesAsync();

            return new Result { Success = true, Message = "Patient deleted successfully" };
        }


        public Task<Patient> GetPatientByIdAsync(int patientId)
        {
            return _db.Patients.FirstOrDefaultAsync(t => t.Id == patientId);
        }

        public async Task<List<Patient>> GetPatientsAsync()
        {
            return await _db.Patients.ToListAsync();
        }

        public async Task<Result> UpdatePatientAsync(Patient patient)
        {

            var patientExist = await _db.Patients.FirstOrDefaultAsync(u => u.Id == patient.Id);

            if (patientExist == null)
            {
                return new Result { Success = false, Message = "Patient not found" };
            }

            var existingUser = await _userManager.FindByIdAsync(patientExist.PatientId);

            if (existingUser != null)
            {
                existingUser.UserName = patient.Email;
                existingUser.FirstName = patient.FirstName;
                existingUser.LastName = patient.LastName;
                existingUser.Email = patient.Email;
                existingUser.PhoneNumber = patient.PhoneNumber;
                existingUser.Gender = patient.Gender;
                existingUser.DOB = patient.DOB;

                await _userManager.UpdateAsync(existingUser);
            }

            _db.ChangeTracker.Clear();
            _db.Patients.Update(patient);
            await _db.SaveChangesAsync();
            return new Result { Success = true, Message = "Updated Success" };
        }

        public async Task<Result> UpdateProfile(int pId, UpdatePatientProfileDTO updatePatientProfileDTO)
        {
            var patient = await _db.Patients.FirstOrDefaultAsync(p => p.Id == pId);

            if (patient == null)
            {
                return new Result { Success = false, Message = "Patient not found." };
            }

            patient.MaritalStatus = updatePatientProfileDTO.MaritalStatus;
            patient.BloodGroup= updatePatientProfileDTO.BloodGroup;
            patient.Address= updatePatientProfileDTO.Address;

            await _db.SaveChangesAsync();

            return new Result { Success = true, Message = "Profile Updated Successfully" };
        }
    }
}
