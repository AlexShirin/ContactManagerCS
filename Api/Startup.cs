using ContactManagerCS.Common.ApiKeyAuthentication;
using ContactManagerCS.DAL.Database;
using ContactManagerCS.DAL.Repositories;
using ContactManagerCS.Services;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace ContactManagerCS;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        string connection = _configuration.GetConnectionString("DefaultConnection")!;

        services.AddDbContext<ContactContext>(options => options.UseNpgsql(connection).LogTo(Console.WriteLine, LogLevel.Information));

        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IContactService, ContactService>();

        services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
        services.AddScoped<ApiKeyAuthFilter>();
        services.AddHttpContextAccessor();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddAutoMapper(typeof(ContactMapper));
        services.AddSwaggerGen(options =>
        {
            var basePath = AppContext.BaseDirectory;
            var xmlPath = Path.Combine(basePath, "ContactManagerCS.xml");

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Contact manager",
                Description = "An ASP.NET Core Web API for managing contact items",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://example.com/license")
                }
            });

            options.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Query,
                Name = "apiKey",
                Type = SecuritySchemeType.ApiKey,
                Description = "API Key Authentication"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "apiKey"
                        },
                        Scheme = "apiKey",
                        Name = "apiKey",
                        In = ParameterLocation.Query
                    },
                    new List<string>()
                }
            });
        });

        services.Configure<HttpLoggingOptions>(_configuration.GetSection("Logging:HttpLogging"));
        services.AddHttpLogging(logging => { });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (_environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseRouting();

        app.UseHttpLogging();

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
