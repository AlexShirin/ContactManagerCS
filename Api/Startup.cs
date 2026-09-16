using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using ContactManagerCS.Common.ApiKeyAuthentication;
using ContactManagerCS.Common.Loggers;
using ContactManagerCS.DAL.Database;
using ContactManagerCS.DAL.Repositories;
using ContactManagerCS.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

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

        services.AddAuthentication("ApiKey").AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);
        services.AddAuthorization();

        //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer(options =>
        //    {
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidateAudience = true,
        //            ValidateLifetime = true,
        //            ValidateIssuerSigningKey = true,
        //            ValidIssuer = "yourdomain.com",
        //            ValidAudience = "yourdomain.com",
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_secret_key_here12345678901234567890"))
        //        };
        //    });

        services.AddControllers();
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = false;
            options.ReportApiVersions = true;
        })
        .AddMvc()
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddEndpointsApiExplorer();
        services.AddAutoMapper(typeof(ContactMapper));
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "X-API-Key",
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
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        services.Configure<HttpLoggingOptions>(_configuration.GetSection("Logging:HttpLogging"));
        services.AddHttpLogging(logging => { });

        services.Configure<RabbitMQOptions>(_configuration.GetSection("RabbitMq"));
        services.AddScoped<ICustomLogger, RabbitMQLogger>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddHostedService<LogServiceHostedService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (_environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpLogging();

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
