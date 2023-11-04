using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XeGo.Services.CodeValue.API.Data;
using XeGo.Services.CodeValue.API.Entities;
using XeGo.Services.CodeValue.API.Models.Dto;
using XeGo.Shared.Lib.Helpers;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.CodeValue.API.Controllers
{
    [ApiController]
    [Route("api/v1/code-values")]
    public class CodeValueController
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private ResponseDto ResponseDto { get; set; }

        public CodeValueController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            ResponseDto = new();
        }

        [HttpGet("by-code-name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCodeName(string codeName, bool? isActive, bool? isEffective)
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
                    return new NotFoundObjectResult(ResponseDto);
                }

                List<Dictionary<string, object>?> result =
                    codeValues
                        .Select(codeValue => ConvertToOriginalObject(codeValue, codeMeta))
                        .Where(originalObject => originalObject != null)
                        .ToList();


                ResponseDto.Data = result;
                ResponseDto.IsSuccess = true;

                return new OkObjectResult(ResponseDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return new ContentResult()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Content = e.Message
                };
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CodeValueDto requestDto)
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
                    return new BadRequestObjectResult(ResponseDto);
                }

                var createDto = _mapper.Map<Entities.CodeValue>(requestDto);
                var codeValueListWithSameCodeName = _dbContext.CodeValues.Where(e => e.Name == requestDto.Name).ToList();
                bool codeValueExists = codeValueListWithSameCodeName.Any(codeValue => ObjectHelpers.Equals(codeValue, createDto));
                if (codeValueExists)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = $"Code value with Name {requestDto.Name} is already existed.";
                    return new BadRequestObjectResult(ResponseDto);
                }

                await _dbContext.CodeValues.AddAsync(createDto);
                await _dbContext.SaveChangesAsync();

                var createdObject = await _dbContext.CodeValues.FirstOrDefaultAsync(e => e.Name == requestDto.Name);

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = createdObject;

                return new CreatedAtActionResult(
                        nameof(GetByCodeName),
                        nameof(CodeValueController).Replace("Controller", ""),
                        new { codeName = requestDto.Name, isActive = requestDto.IsActive },
                        ResponseDto
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return new ContentResult()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Content = e.Message
                };
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit([FromBody] CodeValueDto requestDto)
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
                    return new BadRequestObjectResult(ResponseDto);
                }

                var codeValue = await _dbContext.CodeValues.FirstOrDefaultAsync(e => e.Name == requestDto.Name);
                if (codeValue == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = $"Code value with Name {requestDto.Name} does not exist.";
                    return new BadRequestObjectResult(ResponseDto);
                }

                _mapper.Map(requestDto, codeValue);
                await _dbContext.SaveChangesAsync();

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = codeValue;

                return new OkObjectResult(ResponseDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return new ContentResult()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Content = e.Message
                };
            }
        }

        #region Private Methods

        private CodeValueDto NormalizeCodeValueDto(CodeValueDto inputDto)
        {
            var output = ObjectHelpers.DeepCopy(inputDto);

            output.Name = output.Name.ToUpper();

            return output;
        }

        private Dictionary<string, object>? ConvertToOriginalObject(Entities.CodeValue codeValue, CodeMetaData codeMeta)
        {
            if (codeValue.Name.ToUpper() != codeMeta.Name.ToUpper())
            {
                return null;
            }

            Dictionary<string, object> result = new();
            try
            {
                if (codeValue.Value1 != null && codeMeta is { Value1Name: not null, Value1Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value1, codeMeta.Value1Type);
                    if (returnValue != null)
                    {
                        result[codeMeta.Value1Name] = returnValue;
                    }
                }
                if (codeValue.Value2 != null && codeMeta is { Value2Name: not null, Value2Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value2, codeMeta.Value2Type);
                    if (returnValue != null)
                    {
                        result[codeMeta.Value2Name] = returnValue;
                    }
                }
                if (codeValue.Value3 != null && codeMeta is { Value3Name: not null, Value3Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value3, codeMeta.Value3Type);
                    if (returnValue != null)
                    {
                        result[codeMeta.Value3Name] = returnValue;
                    }
                }
                if (codeValue.Value4 != null && codeMeta is { Value4Name: not null, Value4Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value4, codeMeta.Value4Type);
                    if (returnValue != null)
                    {
                        result[codeMeta.Value4Name] = returnValue;
                    }
                }
                if (codeValue.Value5 != null && codeMeta is { Value5Name: not null, Value5Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value5, codeMeta.Value5Type);
                    if (returnValue != null)
                    {
                        result[codeMeta.Value5Name] = returnValue;
                    }
                }
                if (codeValue.Value6 != null && codeMeta is { Value6Name: not null, Value6Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value6, codeMeta.Value6Type);
                    if (returnValue != null)
                    {
                        result[codeMeta.Value6Name] = returnValue;
                    }
                }
                if (codeValue.Value7 != null && codeMeta is { Value7Name: not null, Value7Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value7, codeMeta.Value7Type);
                    if (returnValue != null)
                    {
                        result[codeMeta.Value7Name] = returnValue;
                    }
                }
                if (codeValue.Value8 != null && codeMeta is { Value8Name: not null, Value8Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value8, codeMeta.Value8Type);
                    if (returnValue != null)
                    {
                        result[codeMeta.Value8Name] = returnValue;
                    }
                }
                if (codeValue.Value9 != null && codeMeta is { Value9Name: not null, Value9Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value9, codeMeta.Value9Type);
                    if (returnValue != null)
                    {
                        result[codeMeta.Value9Name] = returnValue;
                    }
                }
                if (codeValue.Value10 != null && codeMeta is { Value10Name: not null, Value10Type: not null })
                {
                    var returnValue = CovertToType(codeValue.Value10, codeMeta.Value10Type);
                    if (returnValue != null)
                    {
                        result[codeMeta.Value10Name] = returnValue;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return result;
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
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion
    }
}
