using AutoMapper;
using Hospital.Services.Clinic.Models;
using Hospital.Services.Clinic.Models.DTO;

namespace Hospital.Services.Clinic
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<PatientDTO, Patient>().ReverseMap();
                config.CreateMap<PatientDTO,Doctor>().ReverseMap();
                config.CreateMap<PatientAppointmentsDto,PatientAppointments>().ReverseMap();
                config.CreateMap<PrescriptionDTO,Prescription>().ReverseMap();
               
            });

            return mappingConfig;
        }
    }
}
