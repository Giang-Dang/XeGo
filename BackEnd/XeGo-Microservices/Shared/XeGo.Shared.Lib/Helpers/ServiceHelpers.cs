using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace XeGo.Shared.Lib.Helpers
{
    public static class ServiceHelpers
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration.GetValue<string>("ApiSettings:JwtOptions:Secret");
            var issuer = configuration.GetValue<string>("ApiSettings:JwtOptions:Issuer");
            var audience = configuration.GetValue<string>("ApiSettings:JwtOptions:Audience");
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience
                    };
                });
        }

        public static void AddAppDbContext<TContext>(this IServiceCollection services, IConfiguration configuration, string connectionStringName) where TContext : DbContext
        {
            services.AddDbContext<TContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString(connectionStringName));
            });
        }
    }
}
