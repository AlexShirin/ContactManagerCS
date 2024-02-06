using ContactManagerCS.DAL.Database;
using ContactManagerCS.DAL.Repositories;
using ContactManagerCS.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

        services.AddDbContext<ContactContext>(options => options.UseNpgsql(connection));

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

        app.UseHttpsRedirection();

        app.UseEndpoints(endpoints => 
        { 
            endpoints.MapControllers(); 
        });
    }
}
