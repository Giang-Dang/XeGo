using AutoMapper;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using XeGo.Services.CodeValue.Grpc.Data;
using XeGo.Services.CodeValue.Grpc.Entities;
using XeGo.Services.CodeValue.Grpc.Protos;

namespace XeGo.Services.CodeValue.Grpc.Services
{
    public class CodeValueService : CodeValueProtoService.CodeValueProtoServiceBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CodeValueService> _logger;
        private Response Response { get; set; }

        public CodeValueService(AppDbContext dbContext, IMapper mapper, ILogger<CodeValueService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            Response = new();
        }

        public override async Task<Response> GetByCodeName(GetByCodeNameRequest request, ServerCallContext context)
        {
            try
            {
                string codeName = request.CodeName.ToUpper();

                var codeMeta = await _dbContext.CodeMetaData.FirstOrDefaultAsync(e => e.Name == codeName);

                IQueryable<Entities.CodeValue> query = _dbContext.CodeValues.Where(e => e.Name == codeName);

                if (request.IsActive.HasValue && request.IsActive.Value)
                {
                    query = query.Where(e => e.IsActive == request.IsActive.Value);
                }
                if (request.IsEffective.HasValue && request.IsEffective.Value)
                {
                    var utcNow = DateTime.UtcNow;
                    query = query.Where(e => e.EffectiveStartDate <= utcNow && utcNow <= e.EffectiveEndDate);
                }

                var codeValues = await query.ToListAsync();

                if (!codeValues.Any() || codeMeta == null)
                {
                    Response.IsSuccess = true;
                    Response.Data = "";
                    Response.Message = $"Code value with code name {codeName} is not found";
                    return Response;
                }

                List<List<object?>?> result =
                    codeValues
                        .Select(codeValue => ConvertToOriginalObject(codeValue, codeMeta))
                        .Where(originalObject => originalObject != null)
                        .ToList();

                string json = JsonConvert.SerializeObject(result);

                Response.Data = json;
                Response.IsSuccess = true;
            }
            catch (Exception e)
            {
                Response.IsSuccess = false;
                Response.Data = "";
                Response.Message = $"Internal Server Error: {e.Message}";

                _logger.LogError($"Internal Server Error: {e.Message}");
            }
            return Response;
        }

        public override async Task<Response> GetValue2(GetValue2Request request, ServerCallContext context)
        {
            try
            {
                IQueryable<string> query = _dbContext.CodeValues
                    .Where(c => c.Name == request.CodeName && c.Value1 == request.Value1)
                    .Select(c => c.Value2 ?? "");

                var res = await query.ToListAsync();
                Response.Data = res[0];
                Response.IsSuccess = true;
            }
            catch (Exception e)
            {
                Response.IsSuccess = false;
                Response.Data = "";
                Response.Message = $"Internal Server Error: {e.Message}";

                _logger.LogError($"Internal Server Error: {e.Message}");
            }
            return Response;
        }

        #region Private Methods
        private List<object?>? ConvertToOriginalObject(Entities.CodeValue codeValue, CodeMetaData codeMeta)
        {
            if (codeValue.Name.ToUpper() != codeMeta.Name.ToUpper())
            {
                return null;
            }

            List<object?>? results = new();
            try
            {
                if (codeValue.Value1 != null && codeMeta is { Value1Name: not null, Value1Type: not null })
                {
                    var returnValue = ConvertToType(codeValue.Value1, codeMeta.Value1Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value2 != null && codeMeta is { Value2Name: not null, Value2Type: not null })
                {
                    var returnValue = ConvertToType(codeValue.Value2, codeMeta.Value2Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value3 != null && codeMeta is { Value3Name: not null, Value3Type: not null })
                {
                    var returnValue = ConvertToType(codeValue.Value3, codeMeta.Value3Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value4 != null && codeMeta is { Value4Name: not null, Value4Type: not null })
                {
                    var returnValue = ConvertToType(codeValue.Value4, codeMeta.Value4Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value5 != null && codeMeta is { Value5Name: not null, Value5Type: not null })
                {
                    var returnValue = ConvertToType(codeValue.Value5, codeMeta.Value5Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value6 != null && codeMeta is { Value6Name: not null, Value6Type: not null })
                {
                    var returnValue = ConvertToType(codeValue.Value6, codeMeta.Value6Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value7 != null && codeMeta is { Value7Name: not null, Value7Type: not null })
                {
                    var returnValue = ConvertToType(codeValue.Value7, codeMeta.Value7Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value8 != null && codeMeta is { Value8Name: not null, Value8Type: not null })
                {
                    var returnValue = ConvertToType(codeValue.Value8, codeMeta.Value8Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value9 != null && codeMeta is { Value9Name: not null, Value9Type: not null })
                {
                    var returnValue = ConvertToType(codeValue.Value9, codeMeta.Value9Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value10 != null && codeMeta is { Value10Name: not null, Value10Type: not null })
                {
                    var returnValue = ConvertToType(codeValue.Value10, codeMeta.Value10Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CodeValueService)}>{nameof(ConvertToOriginalObject)}: {e.Message}");
                throw;
            }

            return results;
        }
        private dynamic? ConvertToType(string inputValue, string outputType)
        {
            try
            {
                switch (outputType.ToLower())
                {
                    case "int":
                        return Int32.Parse(inputValue);
                    case "string":
                        return inputValue;
                    case "decimal":
                        return Decimal.Parse(inputValue);
                    case "datetime":
                        return DateTime.Parse(inputValue);
                    case "double":
                        return Double.Parse(inputValue);
                    case "float":
                        return float.Parse(inputValue);
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CodeValueService)}>{nameof(ConvertToType)}: {e.Message}");
                return null;
            }
        }

        #endregion
    }

}
