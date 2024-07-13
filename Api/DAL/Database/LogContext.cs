using ContactManagerCS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.DAL.Database;

public class LogContext : DbContext
{
    public DbSet<Log> Logs { get; set; }

    public LogContext(DbContextOptions<LogContext> options) : base(options) 
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactContext).Assembly);
    }
}
