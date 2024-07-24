using System.Threading;
using ContactManagerCS.DAL.Database;
using ContactManagerCS.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ContactManagerCS.DAL.Repositories;

public class LogRepository : ILogRepository
{
    protected readonly DbContext Context;
    protected readonly DbSet<Log> DbSet;

    public LogRepository(ContactContext context)
    {
        Context = context;
        DbSet = context.Log;
    }

    public async Task<Log> Add(Log entity)
    {
        DbSet.Add(entity);
        await Context.SaveChangesAsync();
        return entity;
    }
}
