using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using XeGo.Services.Auth.API.Entities;
using XeGo.Services.Auth.API.Models;
using XeGo.Services.Auth.API.Service.IService;
using XeGo.Shared.GrpcConsumer.Services;
using XeGo.Shared.Lib.Constants;

namespace XeGo.Services.Auth.API.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        private readonly CodeValueGrpcService _codeValueGrpcService;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, CodeValueGrpcService codeValueGrpcService, UserManager<ApplicationUser> userManager)
        {
            _codeValueGrpcService = codeValueGrpcService ?? throw new ArgumentNullException(nameof(codeValueGrpcService));
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value ?? throw new ArgumentNullException(nameof(jwtOptions));
        }

        public async Task<string> GenerateAccessTokenAsync(ApplicationUser applicationUser, string loginApp)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var userRoles = await _userManager.GetRolesAsync(applicationUser);

            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName ?? ""),
                new Claim("loginApp", loginApp)
            };
            foreach (var role in userRoles)
            {
                claimList.Add(new Claim(ClaimTypes.Role, role));
            }

            int daysToExpire = await GetRefreshTokenDaysToExpire();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddDays(daysToExpire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken(int size = 256)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        #region Private Methods

        private async Task<int> GetRefreshTokenDaysToExpire()
        {
            try
            {
                var response = await
                    _codeValueGrpcService.GetByCodeName(TokenConstants.TokenProperty, null, null);

                var dataList = JsonConvert.DeserializeObject<List<List<object?>?>>(response.Data.ToString());
                string? value = null;
                foreach (var innerList in dataList)
                {
                    if (innerList is [string and TokenConstants.RefreshTokenDaysToExpireVariableName, _])
                    {
                        value = innerList[1] as string;
                        break;
                    }
                }

                return Convert.ToInt32(value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        #endregion
    }
}
