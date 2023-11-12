using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using XeGo.Services.Auth.API.Data;
using XeGo.Services.Auth.API.Entities;
using XeGo.Services.Auth.API.Models;
using XeGo.Services.Auth.API.Service;
using XeGo.Services.Auth.API.Service.IService;
using XeGo.Services.CodeValue.Grpc.Protos;
using XeGo.Shared.GrpcConsumer.Services;
using XeGo.Shared.Lib.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAppDbContext<AppDbContext>(builder.Configuration, "DefaultConnection");

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddGrpcClient<CodeValueProtoService.CodeValueProtoServiceClient>(o =>
    o.Address = new Uri(builder.Configuration["GrpcSettings:CodeValueGrpcUrl"])
    );
builder.Services.AddScoped<CodeValueGrpcService>();

// Add logging service
LoggingHelpers loggingHelpers = new();
loggingHelpers.ConfigureLogging(Assembly.GetExecutingAssembly().GetName().Name);
builder.Host.UseSerilog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigration();

app.Run();


#region Private Method
void ApplyMigration()
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }

        scope.Dispose();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}
#endregion Private Method
