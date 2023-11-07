using Microsoft.AspNetCore.Mvc;
using XeGo.Services.Auth.API.Models.Dto;
using XeGo.Services.Auth.API.Service.IService;
using XeGo.Shared.Lib.Helpers;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Auth.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthApiController> _logger;
        protected ResponseDto ResponseDto;

        public AuthApiController(IAuthService authService, ILogger<AuthApiController> logger)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ResponseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            string? requestId = RequestIdHelpers.GetRequestId(HttpContext);
            RequestIdHelpers.MapReqIdToRespHeaders(HttpContext, requestId ?? "");

            _logger.LogInformation("RequestId:{requestId} - {class}>{function}: Received", requestId, nameof(AuthApiController), nameof(Register));
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ResponseDto.IsSuccess = true;
                ResponseDto.Message = errorMessage;
                _logger.LogInformation("RequestId:{requestId} - {class}>{function}: Succeed", requestId, nameof(AuthApiController), nameof(Register));
                return BadRequest(ResponseDto);
            }

            _logger.LogInformation("RequestId:{requestId} - {class}>{function}: Succeed", requestId, nameof(AuthApiController), nameof(Register));
            return Ok(ResponseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            string? requestId = RequestIdHelpers.GetRequestId(HttpContext);
            RequestIdHelpers.MapReqIdToRespHeaders(HttpContext, requestId ?? "");

            _logger.LogInformation("RequestId:{requestId} - {class}>{function}: Received", requestId, nameof(AuthApiController), nameof(Register));

            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = "Username or password is incorrect";
                _logger.LogInformation("RequestId:{requestId} - {class}>{function}: Succeed", requestId,
                    nameof(AuthApiController), nameof(Register));
                return BadRequest(ResponseDto);
            }
            ResponseDto.Data = loginResponse;
            _logger.LogInformation("RequestId:{requestId} - {class}>{function}: Succeed", requestId, nameof(AuthApiController), nameof(Register));
            return Ok(ResponseDto);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = "Error encountered";
                return BadRequest(ResponseDto);
            }
            return Ok(ResponseDto);
        }
    }
}
