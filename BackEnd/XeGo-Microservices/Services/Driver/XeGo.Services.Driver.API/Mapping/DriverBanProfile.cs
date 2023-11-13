using AutoMapper;
using XeGo.Services.Driver.API.Entities;
using XeGo.Services.Driver.API.Models;

namespace XeGo.Services.Driver.API.Mapping
{
    public class DriverBanProfile : Profile
    {
        public DriverBanProfile()
        {
            CreateMap<CreateDriverBanRequestDto, DriverBan>()
                .ReverseMap()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<EditDriverBanRequestDto, DriverBan>()
                .ReverseMap()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
