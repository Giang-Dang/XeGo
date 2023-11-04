using AutoMapper;
using XeGo.Services.CodeValue.API.Entities;
using XeGo.Services.CodeValue.API.Models.Dto;

namespace XeGo.Services.CodeValue.API.Mapping
{
    public class CodeMetaProfile : Profile
    {
        public CodeMetaProfile()
        {
            CreateMap<CodeMetaData, CodeMetaDataDto>().ReverseMap();
        }
    }
}
