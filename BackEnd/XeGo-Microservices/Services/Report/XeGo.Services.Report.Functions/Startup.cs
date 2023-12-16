

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using XeGo.Services.Report.Functions.Data;


[assembly: FunctionsStartup(typeof(XeGo.Services.Report.Functions.Startup))]
namespace XeGo.Services.Report.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var context = builder.GetContext();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(context.ApplicationRootPath)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, context.EnvironmentName, Assembly.GetExecutingAssembly().GetName().Name!))
                .Enrich.WithProperty("Environment", context.EnvironmentName)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            builder.Services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));
        }

        private ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration, string environment, string executingAssemblyName)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{executingAssemblyName.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM-dd}",
                NumberOfReplicas = 1,
                NumberOfShards = 2,
            };
        }

        private void ApplyMigrations(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (db.Database.GetPendingMigrations().Any())
            {
                db.Database.Migrate();
            }
        }
    }
}
