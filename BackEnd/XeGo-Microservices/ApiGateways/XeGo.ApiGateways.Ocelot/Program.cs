using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using System.Reflection;
using XeGo.Shared.Lib.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add logging service
LoggingHelpers loggingHelpers = new();
loggingHelpers.ConfigureLogging(Assembly.GetExecutingAssembly().GetName().Name!);
builder.Host.UseSerilog();

var env = builder.Environment;
builder.Configuration.AddJsonFile($"ocelot.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Ocelot is ready!");
});

app.UseOcelot().Wait();

app.Run();