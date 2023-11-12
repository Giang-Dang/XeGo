using Serilog;
using System.Reflection;
using XeGo.Services.Driver.API.Data;
using XeGo.Services.Driver.API.Repository;
using XeGo.Services.Driver.API.Repository.IRepository;
using XeGo.Shared.Lib.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAppDbContext<AppDbContext>(builder.Configuration, "DefaultConnection");

builder.Services.AddControllers();

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddScoped<IBanRepository, BanRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IDriverInfoRepository, DriverInfoRepository>();
builder.Services.AddScoped<IDriverBanRepository, DriverBanRepository>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
