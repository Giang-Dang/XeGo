using AutoMapper;
using XeGo.Services.Location.API.Entities;
using XeGo.Services.Location.API.Models.Dto;

namespace XeGo.Services.Location.API.Mapping
{
    public class UserLocationProfile : Profile
    {
        public UserLocationProfile()
        {
            CreateMap<DriverLocation, RiderLocationRequestDto>().ReverseMap();
            CreateMap<DriverLocation, DriverLocationResponseDto>().ReverseMap();

        }
    }
}
