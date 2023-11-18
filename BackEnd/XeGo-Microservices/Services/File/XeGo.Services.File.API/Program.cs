using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using XeGo.Services.File.API.Data;
using XeGo.Services.File.API.Services;
using XeGo.Services.File.API.Services.IServices;
using XeGo.Shared.Lib.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(_ =>
    new BlobServiceClient(builder.Configuration.GetValue<string>("BlobConnection"))
);
builder.Services.AddAppDbContext<AppDbContext>(builder.Configuration, "DefaultConnection");
builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddScoped<IImageService, ImageService>();

// Add secret settings
var isDevelopment = builder.Environment.IsDevelopment();

if (isDevelopment)
{
    builder.Configuration.AddUserSecrets<Program>();
}
else
{
    builder.Configuration.AddDockerSecrets();
}

// Add logging service
LoggingHelpers loggingHelpers = new();
loggingHelpers.ConfigureLogging(Assembly.GetExecutingAssembly().GetName().Name!);
builder.Host.UseSerilog();

builder.Services.AddControllers();
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
