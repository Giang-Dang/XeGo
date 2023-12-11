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
            Log.Information($"{nameof(LoggingHelpers)} > ${nameof(ConfigureLogging)} > ${executingAssemblyName} : Triggered");

            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: false)
                    .AddEnvironmentVariables()
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
            catch (Exception e)
            {
                Log.Error($"{nameof(LoggingHelpers)} > ${nameof(ConfigureLogging)} > ${executingAssemblyName} : ${e.Message}");
            }

        }

        public ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment, string executingAssemblyName)
        {
            Log.Information($"{nameof(LoggingHelpers)} > ${nameof(ConfigureElasticSink)} > ${executingAssemblyName} : Triggered");

            var index = $"{executingAssemblyName.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM-dd}";
            var ElasticsearchUri = new Uri(Environment.GetEnvironmentVariable("ElasticConfiguration__Uri"));

            Console.WriteLine(index);
            Console.WriteLine(ElasticsearchUri.ToString());

            return new ElasticsearchSinkOptions(ElasticsearchUri)
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{index}",
                NumberOfReplicas = 1,
                NumberOfShards = 2,
            };
        }
    }
}
