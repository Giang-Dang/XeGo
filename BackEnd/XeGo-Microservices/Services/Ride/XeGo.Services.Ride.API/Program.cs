using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using XeGo.Services.Location.Grpc.Protos;
using XeGo.Services.Price.Grpc.Protos;
using XeGo.Services.Ride.API.Data;
using XeGo.Services.Ride.API.Hubs;
using XeGo.Services.Ride.API.Providers;
using XeGo.Services.Ride.API.Repository;
using XeGo.Services.Ride.API.Repository.IRepository;
using XeGo.Shared.GrpcConsumer.Services;
using XeGo.Shared.Lib.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserIdProvider, SubClaimUserIdProvider>();

builder.Services.AddScoped<IRideRepository, RideRepository>();
builder.Services.AddScoped<ICodeValueRepository, CodeValueRepository>();

builder.Services.AddSignalR();

builder.Services.AddAppDbContext<AppDbContext>(builder.Configuration, "DefaultConnection");

// Add logging service
LoggingHelpers loggingHelpers = new();
loggingHelpers.ConfigureLogging(Assembly.GetExecutingAssembly().GetName().Name!);
builder.Host.UseSerilog();

//Add LocationGrpc service
builder.Services.AddGrpcClient<LocationProtoService.LocationProtoServiceClient>(o =>
    o.Address = new Uri(builder.Configuration["GrpcSettings:LocationGrpcUrl"])
);
builder.Services.AddScoped<LocationGrpcService>();

//Add PriceGrpc Service
builder.Services.AddGrpcClient<PriceProtoService.PriceProtoServiceClient>(o =>
    o.Address = new Uri(builder.Configuration["GrpcSettings:PriceGrpcUrl"])
);
builder.Services.AddScoped<PriceGrpcService>();

//Add VehicleTypePriceGrpc Service
builder.Services.AddGrpcClient<VehicleTypePriceProtoService.VehicleTypePriceProtoServiceClient>(o =>
    o.Address = new Uri(builder.Configuration["GrpcSettings:PriceGrpcUrl"])
);
builder.Services.AddScoped<VehicleTypePriceGrpcService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapHub<RideHub>("/hubs/ride-hub");

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