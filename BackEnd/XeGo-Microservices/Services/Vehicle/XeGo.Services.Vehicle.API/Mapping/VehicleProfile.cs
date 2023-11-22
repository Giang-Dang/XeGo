using AutoMapper;
using XeGo.Services.Vehicle.API.Models;

namespace XeGo.Services.Vehicle.API.Mapping
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<CreateVehicleRequestDto, Entities.Vehicle>().ReverseMap();
            CreateMap<EditVehicleRequestDto, Entities.Vehicle>().ReverseMap();
        }
    }
}
