using AutoMapper;
using XeGo.Services.Driver.API.Entities;
using XeGo.Services.Driver.API.Models;

namespace XeGo.Services.Driver.API.Mapping
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<CreateVehicleRequestDto, Vehicle>()
                .ReverseMap()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<EditVehicleRequestDto, Vehicle>()
                .ReverseMap()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
