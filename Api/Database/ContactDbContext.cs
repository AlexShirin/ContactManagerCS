using ContactManagerCS.Models;

using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Database;

public class ContactDbContext : DbContext
{
    public DbSet<Contact> ContactItems { get; set; } = null!;

    public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=contactDb;Username=postgres;Password=sa");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>().HasData(
            new Contact { Id = 1, Name = "Tom", Email = "a@a.a", Phone = "11", Company = "A" },
            new Contact { Id = 2, Name = "Bob", Email = "b@a.a", Phone = "22", Company = "B" },
            new Contact { Id = 3, Name = "Sam", Email = "c@a.a", Phone = "33", Company = "C" }
        );
    }
}
