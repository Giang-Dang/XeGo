using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using XeGo.Services.Auth.API.Data;
using XeGo.Services.Auth.API.Entities;
using XeGo.Services.Auth.API.Models;
using XeGo.Services.Auth.API.Service.IService;
using XeGo.Shared.Lib.Constants;
using XeGo.Shared.Lib.Helpers;

namespace XeGo.Services.Auth.API.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<JwtTokenGenerator> _logger;
        private readonly AppDbContext _db;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, UserManager<ApplicationUser> userManager, ILogger<JwtTokenGenerator> logger, AppDbContext db)
        {
            _userManager = userManager;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _db = db;
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
                claimList.Add(new Claim(ClaimTypes.Role, role.ToUpper()));
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
                var codeValue = await _db.CodeValues.FirstOrDefaultAsync(c => c.Name == TokenConstants.TokenProperty && c.Value1 == TokenConstants.AccessTokenDaysToExpireVariableName);
                if (codeValue == null)
                {
                    return TokenConstants.DefaultAccessTokenDaysToExpire;
                }

                int value = CodeValueHelpers.GetOriginalValue(codeValue.Value2!, codeValue.Value2Type!);

                return value;
            }
            catch (Exception e)
            {
                _logger.LogError($"{nameof(JwtTokenGenerator)}>{nameof(GetRefreshTokenDaysToExpire)}: {e.Message}");
                return TokenConstants.DefaultAccessTokenDaysToExpire;
            }
        }


        #endregion
    }
}
