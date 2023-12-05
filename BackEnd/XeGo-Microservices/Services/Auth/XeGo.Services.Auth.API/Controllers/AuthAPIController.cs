using Microsoft.AspNetCore.Mvc;
using XeGo.Services.Auth.API.Models.Dto;
using XeGo.Services.Auth.API.Service.IService;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Auth.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController(IAuthService authService, ILogger<AuthApiController> logger)
        : ControllerBase
    {
        private readonly IAuthService _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        private readonly ILogger<AuthApiController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        protected ResponseDto ResponseDto = new();

        [HttpGet("user/rider-type")]
        public async Task<ResponseDto> GetRiderType(string id)
        {
            _logger.LogInformation("{class}>{function}: Received", nameof(AuthApiController), nameof(GetRiderType));

            try
            {
                var type = await _authService.GetRiderType(id);
                if (type == null)
                {
                    ResponseDto.Message = "Not Found!";
                    ResponseDto.IsSuccess = false;
                    return ResponseDto;
                }

                ResponseDto.Data = type;
                ResponseDto.IsSuccess = true;
                _logger.LogInformation("{class}>{function}: Done", nameof(AuthApiController), nameof(GetRiderType));
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(AuthApiController)}>{nameof(GetRiderType)}: {e.Message}");
                ResponseDto.IsSuccess = false;
                ResponseDto.Data = null;
                ResponseDto.Message = e.Message;
            }

            return ResponseDto;
        }

        [HttpPost("user/register")]
        public async Task<ResponseDto> Register([FromBody] RegistrationRequestDto model)
        {
            _logger.LogInformation("{class}>{function}: Received", nameof(AuthApiController), nameof(Register));

            try
            {
                var serviceResponse = await _authService.Register(model);
                if (!serviceResponse.IsSuccess)
                {
                    ResponseDto.IsSuccess = false;
                    ResponseDto.Message = serviceResponse.Message;
                    _logger.LogInformation("{class}>{function}: Registration Failed", nameof(AuthApiController), nameof(Register));
                    return ResponseDto;
                }

                ResponseDto.IsSuccess = true;
                ResponseDto.Data = serviceResponse.Data;
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
                    ResponseDto.Message = "Login Failed";
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

        [HttpPost("user/refresh-token")]
        public async Task<ResponseDto> RefreshToken([FromBody] RefreshTokensRequestDto model)
        {
            var accessToken = await _authService.RefreshToken(model.RefreshToken, model.UserId, model.FromApp);
            if (accessToken == null)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = "Not Found!";
                return ResponseDto;
            }
            ResponseDto.IsSuccess = true;
            ResponseDto.Data = accessToken;
            return ResponseDto;
        }


        [HttpPost("create-role")]
        public ResponseDto CreateRole([FromBody] string roleName)
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
        public ResponseDto RemoveRole([FromBody] string roleName)
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
