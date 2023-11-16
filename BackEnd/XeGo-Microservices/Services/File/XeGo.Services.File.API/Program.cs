using Azure.Storage.Blobs;
using Serilog;
using System.Reflection;
using XeGo.Services.Media.API.Data;
using XeGo.Services.Media.API.Services;
using XeGo.Services.Media.API.Services.IServices;
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

// Add logging service
LoggingHelpers loggingHelpers = new();
loggingHelpers.ConfigureLogging(Assembly.GetExecutingAssembly().GetName().Name);
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
