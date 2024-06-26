using Serilog;

namespace ContactManagerCS;

public static partial class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = GetConfiguration();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        var host = BuildWebHost(configuration, args);

        await host.RunAsync();
    }

    private static IHost BuildWebHost(IConfiguration configuration, string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, config) =>
            {
                config.ReadFrom.Configuration(configuration);
                config.ReadFrom.Services(services);
                config.Enrich.FromLogContext();
            })
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder
                .UseConfiguration(configuration)
                .UseStartup<Startup>();
            })
            .Build();
    }

    private static IConfiguration GetConfiguration()
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
          .AddEnvironmentVariables();

        return builder.Build();
    }
}
