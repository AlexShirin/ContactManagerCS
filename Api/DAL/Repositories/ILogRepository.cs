using ContactManagerCS.DAL.Entities;

namespace ContactManagerCS.DAL.Repositories;

public interface ILogRepository
{
    Task<Log> Add(Log entity);
}
