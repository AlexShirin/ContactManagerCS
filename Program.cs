using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ContactManagerCS.Database;
using ContactManagerCS.Contracts;
using ContactManagerCS.Repositories;

namespace ContactManagerCS;

public static partial class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string connection = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ContactDbContext>(options => options.UseNpgsql(connection));
        builder.Services.AddScoped<IContactRepository, ContactRepository>();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var basePath = AppContext.BaseDirectory;
            var xmlPath = Path.Combine(basePath, "ContactManagerCS.xml");
            options.IncludeXmlComments(xmlPath);

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

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();

        //app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }
}