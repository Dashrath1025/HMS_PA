using AutoMapper;
using Hospital.Services.BedAPI.Models;
using Hospital.Services.BedAPI.Models.DTO;

namespace Hospital.Services.Bed
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<BedCategoryDTO, BedCategory>().ReverseMap();
                config.CreateMap<BedDTO, Beds>().ReverseMap().ReverseMap();
                config.CreateMap<BedAllotmentDTO, BedAllotment>().ReverseMap();
               
            });

            return mappingConfig;
        }
    }
}
