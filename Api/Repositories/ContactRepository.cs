using ContactManagerCS.Contracts;
using ContactManagerCS.Database;
using ContactManagerCS.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly ContactDbContext _contactDbContext;

    public ContactRepository(ContactDbContext contactDbContext)
    {
        _contactDbContext = contactDbContext;
    }

    public async Task<List<Contact>> GetAll()
    {
        return await _contactDbContext.ContactItems.ToListAsync();
    }

    public async Task<Contact?> GetById(int id)
    {
        return await _contactDbContext.ContactItems.FindAsync(id);
    }

    public async Task<Contact> Create(Contact contact)
    {
        _contactDbContext.ContactItems.Add(contact);
        await _contactDbContext.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> Update(Contact exists, Contact contact)
    {
        exists.Id = contact.Id;
        exists.Name = contact.Name;
        exists.Email = contact.Email;
        exists.Phone = contact.Phone;
        exists.Work = contact.Work;

        await _contactDbContext.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> Delete(Contact contact)
    {
        _contactDbContext.ContactItems.Remove(contact);
        await _contactDbContext.SaveChangesAsync();
        return contact;
    }
}
