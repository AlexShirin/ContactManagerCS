using AutoMapper;

namespace ContactManagerCS;

public static partial class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = GetConfiguration();

        var host = BuildWebHost(configuration, args);

        await host.RunAsync();
    }

    private static IHost BuildWebHost(IConfiguration configuration, string[] args)
    {
        return Host.CreateDefaultBuilder(args)
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

public class ContactMapperProfile : Profile
{
    public ContactMapperProfile()
    {
        SourceMemberNamingConvention = ExactMatchNamingConvention.Instance;
        DestinationMemberNamingConvention = ExactMatchNamingConvention.Instance;
    }
}