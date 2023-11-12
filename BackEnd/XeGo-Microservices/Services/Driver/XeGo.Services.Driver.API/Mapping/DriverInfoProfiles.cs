using AutoMapper;
using XeGo.Services.Driver.API.Models;

namespace XeGo.Services.Driver.API.Mapping
{
    public class DriverInfoProfiles : Profile
    {
        public DriverInfoProfiles()
        {
            CreateMap<CreateDriverInfoRequestDto, DriveInfo>().ReverseMap();
        }
    }
}
