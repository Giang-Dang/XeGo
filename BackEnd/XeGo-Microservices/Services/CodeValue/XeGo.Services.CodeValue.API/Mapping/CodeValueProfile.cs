using AutoMapper;
using XeGo.Services.CodeValue.API.Models.Dto;

namespace XeGo.Services.CodeValue.API.Mapping
{
    public class CodeValueProfile : Profile
    {
        public CodeValueProfile()
        {
            CreateMap<Entities.CodeValue, CodeValueDto>().ReverseMap();
        }
    }
}
