using AutoMapper;
using XeGo.Services.Driver.API.Entities;
using XeGo.Services.Driver.API.Models;

namespace XeGo.Services.Driver.API.Mapping
{
    public class VehicleBanProfile : Profile
    {
        public VehicleBanProfile()
        {
            CreateMap<CreateVehicleBanRequestDto, VehicleBan>()
                .ReverseMap()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<EditVehicleBanRequestDto, VehicleBan>()
                .ReverseMap()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
