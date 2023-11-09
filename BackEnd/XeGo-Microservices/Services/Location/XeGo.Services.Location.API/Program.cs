

using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using XeGo.Services.CodeValue.Grpc.Protos;
using XeGo.Services.Location.API.Data;
using XeGo.Services.Location.API.Mapping;
using XeGo.Services.Location.API.Services;
using XeGo.Services.Location.API.Services.IServices;
using XeGo.Shared.GrpcConsumer.Services;
using XeGo.Shared.Lib.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add logging service
LoggingHelpers loggingHelpers = new();
loggingHelpers.ConfigureLogging(Assembly.GetExecutingAssembly().GetName().Name);
builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IGeoHashService, GeoHashService>();

builder.Services.AddAutoMapper(
    typeof(UserLocationProfile)
    );

builder.Services.AddGrpcClient<CodeValueProtoService.CodeValueProtoServiceClient>(o =>
    o.Address = new Uri(builder.Configuration["GrpcSettings:CodeValueGrpcUrl"])
);
builder.Services.AddScoped<CodeValueGrpcService>();

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
    catch (Exception a)
    {
        Console.WriteLine(a);
    }
}

#endregion Private Method