using ContactManagerCS.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.DAL.Repositories
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAll();
        Task<Contact?> GetById(int id);
        Task<Contact> Create(Contact contact);
        Task<Contact> Delete(Contact contact);
        Task<List<Contact>> Find(string keyword);
        Task<Contact> Update(Contact contact);
    }
}
