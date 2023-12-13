using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using XeGo.Services.Vehicle.Grpc.Data;
using XeGo.Services.Vehicle.Grpc.Services;
using XeGo.Shared.Lib.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add logging service
LoggingHelpers loggingHelpers = new();
loggingHelpers.ConfigureLogging(Assembly.GetExecutingAssembly().GetName().Name);
builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<DriverService>();
app.MapGrpcService<VehicleService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
