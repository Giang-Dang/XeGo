using AutoMapper;
using XeGo.Services.CodeValue.Grpc.Entities;
using XeGo.Services.CodeValue.Grpc.Models.Dto;

namespace XeGo.Services.CodeValue.Grpc.Mapping
{
    public class CodeMetaProfile : Profile
    {
        public CodeMetaProfile()
        {
            CreateMap<CodeMetaData, CodeMetaDataDto>().ReverseMap();
        }
    }
}
