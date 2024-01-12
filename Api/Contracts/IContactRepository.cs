using ContactManagerCS.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Contracts
{
    public interface IContactRepository
    {
        Task<Contact> Create(Contact contact);
        Task<Contact> Delete(Contact contact);
        Task<List<Contact>> GetAll();
        Task<Contact?> GetById(int id);
        Task<Contact> Update(Contact exists, Contact contact);
    }
}
