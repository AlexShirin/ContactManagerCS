using System.IO;

using ContactManagerCS.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Database;

public class ContactContext : DbContext
{
    public DbSet<Contact> Contact { get; set; } = null!;

    public ContactContext(DbContextOptions<ContactContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactContext).Assembly);
    }
}
