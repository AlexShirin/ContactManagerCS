using ContactManagerCS.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.DAL.Database;

public class ContactContext : DbContext
{
    public DbSet<Contact> Contact { get; set; }

    public ContactContext(DbContextOptions<ContactContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactContext).Assembly);
    }
}
