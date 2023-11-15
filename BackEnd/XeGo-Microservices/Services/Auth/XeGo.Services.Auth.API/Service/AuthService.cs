using Microsoft.AspNetCore.Identity;
using XeGo.Services.Auth.API.Data;
using XeGo.Services.Auth.API.Entities;
using XeGo.Services.Auth.API.Models.Dto;
using XeGo.Services.Auth.API.Service.IService;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Auth.API.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        }

        public async Task<bool> AssignRoleAsync(string userId, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    return false;
                }

                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveRoleAsync(string userId, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    return false;
                }

                await _userManager.RemoveFromRoleAsync(user, roleName);
                return true;
            }

            return false;
        }

        public bool CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }
            if (_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                return false;

            }

            _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            return true;
        }

        public bool RemoveRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return false;
            }
            if (_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                return false;

            }

            _roleManager.DeleteAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            return true;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.PhoneNumber == loginRequestDto.PhoneNumber);

            bool isValid = await _userManager.CheckPasswordAsync(user ?? new(), loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Tokens = new() };
            }

            //if user was found, generate jwt token
            string accessToken = await _jwtTokenGenerator.GenerateAccessTokenAsync(user, loginRequestDto.FromApp);
            string refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            TokenDto tokenDto = new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            UserDto? userDto = new()
            {
                Email = user.Email,
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Tokens = tokenDto,
            };

            await StoreTokensToDb(user, tokenDto, loginRequestDto.FromApp);

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.UserName,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                PhoneNumber = registrationRequestDto.PhoneNumber,
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
                Address = registrationRequestDto.Address,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.PhoneNumber == registrationRequestDto.PhoneNumber);

                    //UserDto userDto = new()
                    //{
                    //    UserName = userToReturn.UserName,
                    //    Email = userToReturn.Email,
                    //    UserId = userToReturn.Id,
                    //    PhoneNumber = userToReturn.PhoneNumber
                    //};

                    await AssignRoleAsync(userToReturn.Id, registrationRequestDto.Role);

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault()?.Description ?? "Error Encountered";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #region Private Methods

        private async Task StoreTokensToDb(ApplicationUser user, TokenDto tokens, string loginApp)
        {
            await RemoveExistingTokens(user, loginApp);
            await AddNewTokens(user, tokens, loginApp);
        }

        private async Task RemoveExistingTokens(ApplicationUser user, string loginApp)
        {
            // remove existing tokens
            await _userManager.RemoveAuthenticationTokenAsync(user, loginApp, TokenConstants.AccessTokenName);
            await _userManager.RemoveAuthenticationTokenAsync(user, loginApp, TokenConstants.RefreshTokenName);
            await _userManager.RemoveAuthenticationTokenAsync(user, loginApp, TokenConstants.RefreshTokenExpirationName);
        }

        private async Task AddNewTokens(ApplicationUser user, TokenDto tokens, string loginApp)
        {
            // add new tokens
            var expirationDate = DateTime.UtcNow.AddDays(7);
            await _userManager.SetAuthenticationTokenAsync(user, loginApp, TokenConstants.AccessTokenName,
                tokens.AccessToken);
            await _userManager.SetAuthenticationTokenAsync(user, loginApp, TokenConstants.RefreshTokenName,
                tokens.RefreshToken);
            await _userManager.SetAuthenticationTokenAsync(user, loginApp, TokenConstants.RefreshTokenExpirationName,
                expirationDate.ToString("O"));
        }

        #endregion
    }
}
