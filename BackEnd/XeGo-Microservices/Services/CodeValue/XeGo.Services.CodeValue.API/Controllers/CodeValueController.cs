using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using XeGo.Services.CodeValue.API.Data;
using XeGo.Services.CodeValue.API.Entities;
using XeGo.Services.CodeValue.API.Models.Dto;
using XeGo.Shared.Lib.Helpers;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.CodeValue.API.Controllers
{
    [ApiController]
    [Route("api/code-values")]
    public class CodeValueController
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CodeValueController> _logger;
        private ResponseDto ResponseDto { get; set; }

        public CodeValueController(AppDbContext dbContext, IMapper mapper, ILogger<CodeValueController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            ResponseDto = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ResponseDto> GetByCodeName(string codeName, bool? isActive, bool? isEffective)
        {
            try
            {
                codeName = codeName.ToUpper();

                var codeMeta = await _dbContext.CodeMetaData.FirstOrDefaultAsync(e => e.Name == codeName);

                IQueryable<Entities.CodeValue> query = _dbContext.CodeValues.Where(e => e.Name == codeName);

                if (isActive.HasValue)
                {
                    query = query.Where(e => e.IsActive == isActive);
                }
                if (isEffective == true)
                {
                    var utcNow = DateTime.UtcNow;
                    query = query.Where(e => e.EffectiveStartDate <= utcNow && utcNow <= e.EffectiveEndDate);
                }

                var codeValues = await query.ToListAsync();

                if (!codeValues.Any() || codeMeta == null)
                {
                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = null;
                    ResponseDto.Message = $"Code value with code name {codeName} is not found";
                    return ResponseDto;
                }

                List<List<object?>?> result =
                    codeValues
                        .Select(codeValue => ConvertToOriginalObject(codeValue, codeMeta))
                        .Where(originalObject => originalObject != null)
                        .ToList();


                ResponseDto.Data = result;
                ResponseDto.IsSuccess = true;


            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CodeValueController)}>{nameof(GetByCodeName)}: {e.Message}");

                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ResponseDto> Create([FromBody] CodeValueDto requestDto)
        {
            try
            {
                requestDto = NormalizeCodeValueDto(requestDto);

                var codeMeta = await _dbContext.CodeMetaData.FirstOrDefaultAsync(e => e.Name == requestDto.Name);
                if (codeMeta == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = $"Code meta with Name {requestDto.Name} does not exists. Please create that Code meta first before adding Code value";
                    return ResponseDto;
                }


                if (!MatchLength(requestDto, codeMeta))
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "The length of the input CodeValue does not match with the length of CodeMetaData.";
                    return ResponseDto;
                }

                if (!MatchIso8601Format(requestDto, codeMeta))
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "Ensure that the format of the provided CodeValue aligns with the format defined in CodeMetaData, " +
                                          "and that the CodeValue is in ISO 8601 format.";
                    return ResponseDto;
                }

                var createDto = _mapper.Map<Entities.CodeValue>(requestDto);
                var codeValueListWithSameCodeName = _dbContext.CodeValues.Where(e => e.Name == requestDto.Name).ToList();
                bool codeValueExists = codeValueListWithSameCodeName.Any(codeValue => ObjectHelpers.Equals(codeValue, createDto));
                if (codeValueExists)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = $"Code value with Name {requestDto.Name} is already existed.";
                    return ResponseDto;
                }

                await _dbContext.CodeValues.AddAsync(createDto);
                await _dbContext.SaveChangesAsync();

                var createdObject = await _dbContext.CodeValues.FirstOrDefaultAsync(e => e.Name == requestDto.Name);

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = createdObject;

            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CodeValueController)}>{nameof(Create)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ResponseDto> Edit([FromBody] CodeValueDto requestDto)
        {
            try
            {
                requestDto = NormalizeCodeValueDto(requestDto);

                var codeMeta = await _dbContext.CodeMetaData.FirstOrDefaultAsync(e => e.Name == requestDto.Name);
                if (codeMeta == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = $"Code meta with Name {requestDto.Name} does not exists. Please create that Code meta first before editing Code value";
                    return ResponseDto;

                }

                var codeValue = await _dbContext.CodeValues.FirstOrDefaultAsync(e => e.Name == requestDto.Name);
                if (codeValue == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = $"Code value with Name {requestDto.Name} does not exist.";
                    return ResponseDto;
                }

                _mapper.Map(requestDto, codeValue);
                await _dbContext.SaveChangesAsync();

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = codeValue;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CodeValueController)}>{nameof(Edit)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }
            return ResponseDto;
        }

        #region Private Methods

        private CodeValueDto NormalizeCodeValueDto(CodeValueDto inputDto)
        {
            var output = ObjectHelpers.DeepCopy(inputDto);

            output.Name = output.Name.ToUpper();

            return output;
        }

        private List<object?>? ConvertToOriginalObject(Entities.CodeValue codeValue, CodeMetaData codeMeta)
        {
            if (codeValue.Name.ToUpper() != codeMeta.Name.ToUpper())
            {
                return null;
            }

            List<object> results = new();
            try
            {
                if (codeValue.Value1 != null && codeMeta is { Value1Name: not null, Value1Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value1, codeMeta.Value1Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value2 != null && codeMeta is { Value2Name: not null, Value2Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value2, codeMeta.Value2Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value3 != null && codeMeta is { Value3Name: not null, Value3Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value3, codeMeta.Value3Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value4 != null && codeMeta is { Value4Name: not null, Value4Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value4, codeMeta.Value4Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value5 != null && codeMeta is { Value5Name: not null, Value5Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value5, codeMeta.Value5Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value6 != null && codeMeta is { Value6Name: not null, Value6Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value6, codeMeta.Value6Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value7 != null && codeMeta is { Value7Name: not null, Value7Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value7, codeMeta.Value7Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value8 != null && codeMeta is { Value8Name: not null, Value8Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value8, codeMeta.Value8Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value9 != null && codeMeta is { Value9Name: not null, Value9Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value9, codeMeta.Value9Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }
                if (codeValue.Value10 != null && codeMeta is { Value10Name: not null, Value10Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value10, codeMeta.Value10Type);
                    if (returnValue != null)
                    {
                        results.Add(returnValue);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return results;
        }

        private dynamic? CovertToType(string inputValue, string outputType)
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
                        return Decimal.Parse(inputValue, CultureInfo.InvariantCulture);
                    case "datetime":
                        return DateTime.Parse(inputValue, null, DateTimeStyles.RoundtripKind);
                    case "double":
                        return Double.Parse(inputValue, CultureInfo.InvariantCulture);
                    case "float":
                        return float.Parse(inputValue, CultureInfo.InvariantCulture);
                    case "bool":
                        return Boolean.Parse(inputValue);
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private bool MatchLength(CodeValueDto codeValue, CodeMetaData codeMeta)
        {
            if (codeValue.Value1 == null ^ codeMeta.Value1Name == null)
            {
                return false;
            }

            if (codeValue.Value2 == null ^ codeMeta.Value2Name == null)
            {
                return false;
            }

            if (codeValue.Value3 == null ^ codeMeta.Value3Name == null)
            {
                return false;
            }

            if (codeValue.Value4 == null ^ codeMeta.Value4Name == null)
            {
                return false;
            }

            if (codeValue.Value5 == null ^ codeMeta.Value5Name == null)
            {
                return false;
            }

            if (codeValue.Value6 == null ^ codeMeta.Value6Name == null)
            {
                return false;
            }

            if (codeValue.Value7 == null ^ codeMeta.Value7Name == null)
            {
                return false;
            }

            if (codeValue.Value8 == null ^ codeMeta.Value8Name == null)
            {
                return false;
            }

            if (codeValue.Value9 == null ^ codeMeta.Value9Name == null)
            {
                return false;
            }

            if (codeValue.Value10 == null ^ codeMeta.Value10Name == null)
            {
                return false;
            }

            return true;
        }

        private bool MatchIso8601Format(CodeValueDto codeValue, CodeMetaData codeMeta)
        {
            if (codeValue.Value1 != null && codeMeta.Value1Type != null)
            {
                if (!IsConvertableToIso8601Format(codeValue.Value1, codeMeta.Value1Type))
                {
                    return false;
                }
            }

            if (codeValue.Value2 != null && codeMeta.Value2Type != null)
            {
                if (!IsConvertableToIso8601Format(codeValue.Value2, codeMeta.Value2Type))
                {
                    return false;
                }
            }

            if (codeValue.Value3 != null && codeMeta.Value3Type != null)
            {
                if (!IsConvertableToIso8601Format(codeValue.Value3, codeMeta.Value3Type))
                {
                    return false;
                }
            }

            if (codeValue.Value4 != null && codeMeta.Value4Type != null)
            {
                if (!IsConvertableToIso8601Format(codeValue.Value4, codeMeta.Value4Type))
                {
                    return false;
                }
            }

            if (codeValue.Value5 != null && codeMeta.Value5Type != null)
            {
                if (!IsConvertableToIso8601Format(codeValue.Value5, codeMeta.Value5Type))
                {
                    return false;
                }
            }

            if (codeValue.Value6 != null && codeMeta.Value6Type != null)
            {
                if (!IsConvertableToIso8601Format(codeValue.Value6, codeMeta.Value6Type))
                {
                    return false;
                }
            }

            if (codeValue.Value7 != null && codeMeta.Value7Type != null)
            {
                if (!IsConvertableToIso8601Format(codeValue.Value7, codeMeta.Value7Type))
                {
                    return false;
                }
            }

            if (codeValue.Value8 != null && codeMeta.Value8Type != null)
            {
                if (!IsConvertableToIso8601Format(codeValue.Value8, codeMeta.Value8Type))
                {
                    return false;
                }
            }

            if (codeValue.Value9 != null && codeMeta.Value9Type != null)
            {
                if (!IsConvertableToIso8601Format(codeValue.Value9, codeMeta.Value9Type))
                {
                    return false;
                }
            }

            if (codeValue.Value10 != null && codeMeta.Value10Type != null)
            {
                if (!IsConvertableToIso8601Format(codeValue.Value10, codeMeta.Value10Type))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsConvertableToIso8601Format(string value, string type)
        {
            switch (type.ToLower())
            {
                case "int":
                    int intResult;
                    return Int32.TryParse(value, out intResult);
                case "string":
                    return true;
                case "decimal":
                    decimal decimalResult;
                    return Decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimalResult);
                case "datetime":
                    DateTime dateResult;
                    return DateTime.TryParseExact(value, "s", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);
                case "double":
                    double doubleResult;
                    return Double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleResult);
                case "float":
                    float floatResult;
                    return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out floatResult);
                case "bool":
                    bool boolResult;
                    return Boolean.TryParse(value, out boolResult);
                default:
                    return false;
            }
        }

        #endregion
    }
}
