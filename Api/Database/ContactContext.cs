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
        //modelBuilder.Entity<Contact>().HasData(
        //    new Contact { Id = 1, Name = "Tom", Email = "a@a.a", Phone = "11", Company = "A" },
        //    new Contact { Id = 2, Name = "Bob", Email = "b@a.a", Phone = "22", Company = "B" },
        //    new Contact { Id = 3, Name = "Sam", Email = "c@a.a", Phone = "33", Company = "C" }
        //);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactContext).Assembly);
    }
}
