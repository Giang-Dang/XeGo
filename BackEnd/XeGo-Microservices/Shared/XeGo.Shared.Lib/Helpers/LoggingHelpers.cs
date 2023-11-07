using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace XeGo.Shared.Lib.Helpers
{
    public class LoggingHelpers
    {
        public void ConfigureLogging(string executingAssemblyName)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: false)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment, executingAssemblyName))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment, string executingAssemblyName)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{executingAssemblyName.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM-dd}",
                NumberOfReplicas = 1,
                NumberOfShards = 2,
            };
        }
    }
}
