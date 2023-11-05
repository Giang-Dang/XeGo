using AutoMapper;
using XeGo.Services.CodeValue.Grpc.Models.Dto;

namespace XeGo.Services.CodeValue.Grpc.Mapping
{
    public class CodeValueProfile : Profile
    {
        public CodeValueProfile()
        {
            CreateMap<Entities.CodeValue, CodeValueDto>().ReverseMap();
        }
    }
}
