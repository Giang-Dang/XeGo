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
    [Route("api/code-meta")]
    [ApiController]
    public class CodeMetaController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CodeMetaController> _logger;
        private ResponseDto ResponseDto { get; set; }

        public CodeMetaController(AppDbContext dbContext, IMapper mapper, ILogger<CodeMetaController> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            ResponseDto = new();
        }


        [HttpGet]
        public async Task<ResponseDto> GetByCodeName(string codeName)
        {
            string? requestId = RequestIdHelpers.GetRequestId(HttpContext);

            _logger.LogInformation($"{requestId}");
            try
            {
                codeName = codeName.ToUpper();

                var codeMeta = await _dbContext.CodeMetaData.FirstOrDefaultAsync(e => e.Name == codeName);
                if (codeMeta == null)
                {
                    ResponseDto.IsSuccess = true;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "Not found!";

                    return ResponseDto;
                }

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = codeMeta;

            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CodeMetaController)}>{nameof(GetByCodeName)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;

        }

        [HttpPost]
        public async Task<ResponseDto> Create([FromBody] CodeMetaDataDto requestDto)
        {
            try
            {
                requestDto = NormalizeCodeMetaDataDto(requestDto);

                var codeMetaExists = await _dbContext.CodeMetaData.AnyAsync(e => e.Name == requestDto.Name);
                if (codeMetaExists)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "Already Existed";

                    return ResponseDto;
                }

                var createObject = _mapper.Map<CodeMetaData>(requestDto);
                await _dbContext.CodeMetaData.AddAsync(createObject);
                await _dbContext.SaveChangesAsync();

                var createdObject = await _dbContext.CodeMetaData.FirstOrDefaultAsync(e => e.Name == requestDto.Name);

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = createdObject;

            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CodeMetaController)}>{nameof(GetByCodeName)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPut]
        public async Task<ResponseDto> Edit([FromBody] CodeMetaDataDto requestDto)
        {
            try
            {
                requestDto = NormalizeCodeMetaDataDto(requestDto);
                var codeMeta = await _dbContext.CodeMetaData.FirstOrDefaultAsync(e => e.Name == requestDto.Name);
                if (codeMeta != null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Data = null;
                    ResponseDto.Message = "This code meta does not exist.";

                    return ResponseDto;
                }

                _mapper.Map(requestDto, codeMeta);
                await _dbContext.SaveChangesAsync();

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = codeMeta;

            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(CodeMetaController)}>{nameof(GetByCodeName)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        #region Private Methods
        private CodeMetaDataDto NormalizeCodeMetaDataDto(CodeMetaDataDto inputDto)
        {
            var output = ObjectHelpers.DeepCopy(inputDto);

            output.Name = output.Name.ToUpper();
            output.Value1Type = output.Value1Type?.ToUpper();
            output.Value2Type = output.Value2Type?.ToUpper();
            output.Value3Type = output.Value3Type?.ToUpper();
            output.Value4Type = output.Value4Type?.ToUpper();
            output.Value5Type = output.Value5Type?.ToUpper();
            output.Value6Type = output.Value6Type?.ToUpper();
            output.Value7Type = output.Value7Type?.ToUpper();
            output.Value8Type = output.Value8Type?.ToUpper();
            output.Value9Type = output.Value9Type?.ToUpper();
            output.Value10Type = output.Value10Type?.ToUpper();

            output.Value1Name = output.Value1Name?.ToUpper();
            output.Value2Name = output.Value2Name?.ToUpper();
            output.Value3Name = output.Value3Name?.ToUpper();
            output.Value4Name = output.Value4Name?.ToUpper();
            output.Value5Name = output.Value5Name?.ToUpper();
            output.Value6Name = output.Value6Name?.ToUpper();
            output.Value7Name = output.Value7Name?.ToUpper();
            output.Value8Name = output.Value8Name?.ToUpper();
            output.Value9Name = output.Value9Name?.ToUpper();
            output.Value10Name = output.Value10Name?.ToUpper();

            return output;
        }
        #endregion
    }
}
