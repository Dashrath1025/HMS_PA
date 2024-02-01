using Hospital.Services.Clinic.Data;
using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Models.DTO;
using Hospital.Services.Clinic.Services.IService;
using Hospital.Services.ClinicAPI.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Hospital.Services.Clinic.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DoctorService(AppDbContext db, UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task AddDoctorAsync(Doctor doctor)
        {
            await _db.Doctors.AddAsync(doctor);
            await _db.SaveChangesAsync();
        }

        public async Task<Result> DeleteDoctorAsync(int doctorId)
        {
            var doctor = await _db.Doctors.FindAsync(doctorId);

            if (doctor == null)
            {
                return new Result { Success = false, Message = "Doctor not found" };
            }

            var user = await _userManager.FindByIdAsync(doctor.DoctorId);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return new Result { Success = false, Message = "Failed to delete corresponding ApplicationUser" };
                }
            }

            _db.Doctors.Remove(doctor);
            await _db.SaveChangesAsync();

            return new Result { Success = true, Message = "Doctor deleted successfully" };
        }

        public async Task<Doctor> GetDoctorByIdAsync(int doctorId)
        {
            return await _db.Doctors.FindAsync(doctorId);
        }

        public async Task<List<Doctor>> GetDoctorsAsync()
        {
            return await _db.Doctors.ToListAsync();
        }

        public async Task<string> RegisterService(PatientDTO patientDTO)


        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["token"];

            var authUrl = _configuration.GetValue<string>("GetAuthAPI:url");

            var fullurl= authUrl + $"api/Auth/register";

            using(HttpClient httpClient  = _httpClientFactory.CreateClient())

            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var content= new StringContent(JsonConvert.SerializeObject(patientDTO),Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(fullurl, content);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string responseContent= await httpResponseMessage.Content.ReadAsStringAsync();
                    return responseContent; 
                }
                else
                {
                    return $"Error: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}";
                }
            }
        }

        public async Task<Result> UpdateDoctorAsync(Doctor existingDoctor)
        {
            var existingDoctorPresent = await _db.Doctors.FirstOrDefaultAsync(u => u.Id == existingDoctor.Id);

            if (existingDoctorPresent == null)
            {
                return new Result { Success = false, Message = "Doctor not found" };
            }


            var existingUser = await _userManager.FindByIdAsync(existingDoctorPresent.DoctorId);

            if (existingUser != null)
            {
                existingUser.UserName = existingDoctor.Email;
                existingUser.FirstName = existingDoctor.FirstName;
                existingUser.LastName = existingDoctor.LastName;
                existingUser.Email = existingDoctor.Email;
                existingUser.PhoneNumber = existingDoctor.PhoneNumber;
                existingUser.Gender = existingDoctor.Gender;
                existingUser.DOB = existingDoctor.DOB;

                await _userManager.UpdateAsync(existingUser);
            }

              _db.ChangeTracker.Clear();
            _db.Doctors.Update(existingDoctor);
            await _db.SaveChangesAsync();
            return new Result { Success = true, Message = "Updated Success" };
        }

        public async Task<Result> UpdateProfile(int dId, UpdateDoctorProfileDTO updateDoctorProfileDTO)
        {
            var doctor = await _db.Doctors.FirstOrDefaultAsync(p => p.Id == dId);

            if (doctor == null)
            {
                return new Result { Success = false, Message = "Doctor not found." };
            }

            doctor.Designation = updateDoctorProfileDTO.Designation;
            doctor.Fees = updateDoctorProfileDTO.Fees;
            doctor.Address = updateDoctorProfileDTO.Address;

            await _db.SaveChangesAsync();

            return new Result { Success = true, Message = "Profile Updated Successfully" };
        }
    }
}
