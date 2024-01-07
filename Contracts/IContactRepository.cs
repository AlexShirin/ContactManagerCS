using ContactManagerCS.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Contracts;

public interface IContactRepository
{
    public Task<List<Contact>> GetAll();
    public Task<Contact> GetById(int id);
    public Task<Contact> Create(Contact item);
    public Task<Contact> Update(Contact item);
    public Task<Contact> DeleteById(int id);
}
