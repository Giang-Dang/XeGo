using Microsoft.AspNetCore.SignalR;
using Serilog;
using System.Reflection;
using XeGo.Services.Location.Grpc.Protos;
using XeGo.Services.Ride.API.Data;
using XeGo.Services.Ride.API.Hubs;
using XeGo.Services.Ride.API.Providers;
using XeGo.Shared.GrpcConsumer.Services;
using XeGo.Shared.Lib.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserIdProvider, SubClaimUserIdProvider>();

builder.Services.AddSignalR();

builder.Services.AddGrpcClient<LocationProtoService.LocationProtoServiceClient>(o =>
    o.Address = new Uri(builder.Configuration["GrpcSettings:LocationGrpcUrl"])
);
builder.Services.AddScoped<LocationGrpcService>();

builder.Services.AddAppDbContext<AppDbContext>(builder.Configuration, "DefaultConnection");

// Add logging service
LoggingHelpers loggingHelpers = new();
loggingHelpers.ConfigureLogging(Assembly.GetExecutingAssembly().GetName().Name!);
builder.Host.UseSerilog();

//Add LocationGrpc service
builder.Services.AddGrpcClient<LocationProtoService.LocationProtoServiceClient>(o =>
    o.Address = new Uri(builder.Configuration["GrpcSettings:LocationGrpcUrl"])
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapHub<RideHub>("/hubs/ride-hub/");

app.MapControllers();

app.Run();
