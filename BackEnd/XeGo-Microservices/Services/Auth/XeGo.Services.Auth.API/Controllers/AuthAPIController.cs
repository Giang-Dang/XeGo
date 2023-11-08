using Microsoft.AspNetCore.Mvc;
using XeGo.Services.Auth.API.Models.Dto;
using XeGo.Services.Auth.API.Service.IService;
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
        public async Task<ResponseDto> Register([FromBody] RegistrationRequestDto model)
        {
            _logger.LogInformation("{class}>{function}: Received", nameof(AuthApiController), nameof(Register));

            try
            {
                var errorMessage = await _authService.Register(model);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = errorMessage;
                    _logger.LogInformation("{class}>{function}: Registration Failed", nameof(AuthApiController), nameof(Register));
                    return ResponseDto;
                }

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = null;
                ResponseDto.Message = "Registration succeed";
                _logger.LogInformation("{class}>{function}: Registration Succeed", nameof(AuthApiController), nameof(Register));
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(AuthApiController)}>{nameof(Register)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost("login")]
        public async Task<ResponseDto> Login([FromBody] LoginRequestDto model)
        {
            _logger.LogInformation("{class}>{function}: Received", nameof(AuthApiController), nameof(Register));

            try
            {
                var loginResponse = await _authService.Login(model);
                if (loginResponse.User == null)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = "Username or password is incorrect";
                    _logger.LogInformation("{class}>{function}: Login Failed",
                        nameof(AuthApiController), nameof(Register));
                    return ResponseDto;
                }

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = loginResponse;
                _logger.LogInformation("{class}>{function}: Login Succeed", nameof(AuthApiController), nameof(Register));

            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(AuthApiController)}>{nameof(Register)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;

        }

        [HttpPost("assign-role")]
        public async Task<ResponseDto> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = "Error encountered";
                return ResponseDto;

            }
            return ResponseDto;

        }
    }
}
