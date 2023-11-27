using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XeGo.Services.Notifications.Functions.Data;

var connectionString = Environment.GetEnvironmentVariable("SqlDbConnection");

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddDbContext<AppDbContext>(
            options => options
                .UseSqlServer(connectionString, providerOptions => providerOptions.EnableRetryOnFailure())
        );
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
.Build();

ApplyMigrations();

host.Run();

void ApplyMigrations()
{
    try
    {
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
            Console.WriteLine("DB migration completed!");
        }

        scope.Dispose();
    }
    catch (Exception e)
    {
        Console.WriteLine("DB migration failed!");
        Console.WriteLine(e);
    }
}
