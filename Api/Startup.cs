using ContactManagerCS.DAL.Database;
using ContactManagerCS.DAL.Repositories;
using ContactManagerCS.Services;

using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
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

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddAutoMapper(typeof(ContactMapperProfile));
        services.AddSwaggerGen(options =>
        {
            var basePath = AppContext.BaseDirectory;
            var xmlPath = Path.Combine(basePath, "ContactManagerCS.xml");
            //options.IncludeXmlComments(xmlPath);

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
        });
        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            logging.RequestHeaders.Add(HeaderNames.Accept);
            logging.RequestHeaders.Add(HeaderNames.ContentType);
            logging.RequestHeaders.Add(HeaderNames.ContentDisposition);
            logging.RequestHeaders.Add(HeaderNames.ContentEncoding);
            logging.RequestHeaders.Add(HeaderNames.ContentLength);

            logging.MediaTypeOptions.AddText("application/json");
            logging.MediaTypeOptions.AddText("multipart/form-data");

            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
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
