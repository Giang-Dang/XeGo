using XeGo.Services.CodeValue.Grpc.Protos;

namespace XeGo.Shared.GrpcConsumer.Services
{
    public class CodeValueGrpcService
    {
        private readonly CodeValueProtoService.CodeValueProtoServiceClient _service;

        public CodeValueGrpcService(CodeValueProtoService.CodeValueProtoServiceClient service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task<Response> GetByCodeName(string codeName, bool? isActive, bool? isEffective)
        {
            var request = new GetByCodeNameRequest
            {
                CodeName = codeName,
                IsActive = isActive,
                IsEffective = isEffective,
            };

            return await _service.GetByCodeNameAsync(request);
        }

        public async Task<Response> GetValue2(string codeName, string value1, bool? isActive, bool? isEffective)
        {
            var request = new GetValue2Request
            {
                CodeName = codeName,
                Value1 = value1,
                IsActive = isActive,
                IsEffective = isEffective,
            };

            return _service.GetValue2(request);
        }
    }
}
