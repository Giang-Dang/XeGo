using Microsoft.AspNetCore.Mvc;
using XeGo.Services.Auth.AuthAPI.Models.Dto;
using XeGo.Services.Auth.AuthAPI.Service.IService;

namespace XeGo.Services.Auth.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto ResponseDto;

        public AuthApiController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            ResponseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ResponseDto.IsSuccess = true;
                ResponseDto.Message = errorMessage;
                return BadRequest(ResponseDto);
            }
            return Ok(ResponseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Message = "Username or password is incorrect";
                return BadRequest(ResponseDto);
            }
            ResponseDto.Result = loginResponse;
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
