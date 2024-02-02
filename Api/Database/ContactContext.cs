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
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=contactDb;Username=postgres;Password=sa");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactContext).Assembly);
    }
}
