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

        [HttpPost("user/register")]
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

        [HttpPost("user/login")]
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

        [HttpPost("user/assign-role")]
        public async Task<ResponseDto> AssignUserToRole([FromBody] AssignRoleRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRoleAsync(model.UserId, model.RoleName.ToUpper());
            if (!assignRoleSuccessful)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = "Error encountered";
                return ResponseDto;

            }
            return ResponseDto;

        }

        [HttpPost("user/remove-role")]
        public async Task<ResponseDto> RemoveUserFromRole([FromBody] RemoveUserFromRoleRequestDto model)
        {
            var removeRoleSuccessful = await _authService.RemoveRoleAsync(model.UserId, model.RoleName.ToUpper());
            if (!removeRoleSuccessful)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = "Error encountered";
                return ResponseDto;
            }
            return ResponseDto;
        }


        [HttpPost("create-role")]
        public async Task<ResponseDto> CreateRole([FromBody] string roleName)
        {
            var roleCreatedSuccessfully = _authService.CreateRole(roleName.ToUpper());
            if (!roleCreatedSuccessfully)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = "Error encountered";
                return ResponseDto;
            }
            return ResponseDto;
        }

        [HttpPost("remove-role")]
        public async Task<ResponseDto> RemoveRole([FromBody] string roleName)
        {
            var roleRemovedSuccessfully = _authService.RemoveRole(roleName.ToUpper());
            if (!roleRemovedSuccessfully)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = "Error encountered";
                return ResponseDto;
            }
            return ResponseDto;
        }

    }
}
