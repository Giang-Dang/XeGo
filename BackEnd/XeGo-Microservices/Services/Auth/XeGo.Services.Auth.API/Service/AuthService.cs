using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XeGo.Services.Auth.API.Data;
using XeGo.Services.Auth.API.Entities;
using XeGo.Services.Auth.API.Models.Dto;
using XeGo.Services.Auth.API.Service.IService;
using XeGo.Shared.GrpcConsumer.Services;
using XeGo.Shared.Lib.Constants;
using XeGo.Shared.Lib.Models;

namespace XeGo.Services.Auth.API.Service
{
    public class AuthService(AppDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator
            , DriverGrpcService driverGrpcService)
        : IAuthService
    {
        private readonly AppDbContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly UserManager<ApplicationUser> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly RoleManager<IdentityRole> _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));

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

            var haveRightRole = false;
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                if (loginRequestDto.FromApp.ToUpper() == userRole.ToUpper())
                {
                    haveRightRole = true;
                    break;
                }

                if (loginRequestDto.FromApp.ToUpper() == RoleConstants.Admin &&
                    userRole.ToUpper() == RoleConstants.Staff)
                {
                    haveRightRole = true;
                    break;
                }
            }

            if (!haveRightRole)
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
                Roles = userRoles.ToList(),
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Tokens = tokenDto
            };

            await StoreTokensToDb(user, tokenDto, loginRequestDto.FromApp);

            return loginResponseDto;
        }

        public async Task<ResponseDto> Register(RegistrationRequestDto requestDto)
        {
            var responseDto = new ResponseDto();

            try
            {
                ApplicationUser user = new()
                {
                    UserName = requestDto.UserName,
                    Email = requestDto.Email,
                    NormalizedEmail = requestDto.Email?.ToUpper(),
                    PhoneNumber = requestDto.PhoneNumber,
                    FirstName = requestDto.FirstName,
                    LastName = requestDto.LastName,
                    Address = requestDto.Address,
                };

                if (requestDto.Email != null)
                {
                    var emailExists = await _db.ApplicationUsers.AnyAsync(u =>
                        u.NormalizedEmail == requestDto.Email.ToUpper());
                    if (emailExists)
                    {
                        responseDto.Message = "This email already exists!";
                        responseDto.IsSuccess = false;

                        return responseDto;
                    }
                }

                var userNameExists = await _db.ApplicationUsers.AnyAsync(u =>
                    u.NormalizedUserName == requestDto.UserName.ToUpper());
                if (userNameExists)
                {
                    responseDto.Message = "This username already exists!";
                    responseDto.IsSuccess = false;

                    return responseDto;
                }

                var phoneNumberExists = await _db.ApplicationUsers.AnyAsync(u =>
                    u.PhoneNumber == requestDto.PhoneNumber);
                if (phoneNumberExists)
                {
                    responseDto.Message = "This phone number already exists!";
                    responseDto.IsSuccess = false;

                    return responseDto;
                }

                var result = await _userManager.CreateAsync(user, requestDto.Password);

                if (!result.Succeeded)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = result.Errors.FirstOrDefault()?.Description ?? "Error Encountered";
                    return responseDto;
                }

                var userToReturn = _db.ApplicationUsers.First(u => u.PhoneNumber == requestDto.PhoneNumber);

                await AssignRoleAsync(userToReturn.Id, requestDto.Role);

                if (requestDto.Role.ToUpper() == RoleConstants.Rider)
                {
                    var riderType = new Rider()
                    {
                        RiderId = userToReturn.Id,
                        CreatedBy = userToReturn.Id,
                        LastModifiedBy = userToReturn.Id,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.UtcNow
                    };
                    await _db.Riders.AddAsync(riderType);
                    await _db.SaveChangesAsync();
                }

                if (requestDto.Role.ToUpper() == RoleConstants.Driver)
                {
                    await driverGrpcService.CreateDriver(
                        userToReturn.Id,
                        userToReturn.UserName!,
                        userToReturn.FirstName,
                        userToReturn.LastName,
                        userToReturn.PhoneNumber!,
                        userToReturn.Email!,
                        userToReturn.Address,
                        userToReturn.Id
                        );
                }

                responseDto.IsSuccess = true;
                responseDto.Message = "";
                responseDto.Data = userToReturn;
                return responseDto;

            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
                return responseDto;
            }
        }

        public async Task<string?> RefreshToken(string refreshToken, string userId, string loginApp)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }
            var existingRefreshToken =
                await _userManager.GetAuthenticationTokenAsync(user, loginApp, TokenConstants.RefreshTokenName);

            if (existingRefreshToken == null)
            {
                return null;
            }

            var newAccessToken = await _jwtTokenGenerator.GenerateAccessTokenAsync(user, loginApp);
            await ReplaceAccessToken(user, newAccessToken, loginApp);
            return newAccessToken;
        }

        public async Task<string?> GetRiderType(string riderId)
        {
            var cRider = await _db.Riders.FirstOrDefaultAsync(r => r.RiderId == riderId);
            if (cRider == null)
            {
                return null;
            }

            return cRider.Type;
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
        }

        private async Task ReplaceAccessToken(ApplicationUser user, string newAccessToken, string loginApp)
        {
            await _userManager.RemoveAuthenticationTokenAsync(user, loginApp, TokenConstants.AccessTokenName);
            await _userManager.SetAuthenticationTokenAsync(user, loginApp, TokenConstants.AccessTokenName,
                newAccessToken);
        }

        private async Task AddNewTokens(ApplicationUser user, TokenDto tokens, string loginApp)
        {
            // add new tokens
            await _userManager.SetAuthenticationTokenAsync(user, loginApp, TokenConstants.AccessTokenName,
                tokens.AccessToken);
            await _userManager.SetAuthenticationTokenAsync(user, loginApp, TokenConstants.RefreshTokenName,
                tokens.RefreshToken); ;
        }

        #endregion
    }
}
