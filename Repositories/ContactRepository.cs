using ContactManagerCS.Contracts;
using ContactManagerCS.Database;
using ContactManagerCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly ContactDbContext ContactDbContext;

    public ContactRepository(ContactDbContext contactDbContext)
    {
        ContactDbContext = contactDbContext;
    }

    public async Task<List<Contact>> GetAll()
    {
        return await ContactDbContext.ContactItems.ToListAsync();
    }

    public async Task<Contact> GetById(int id)
    {
        var item = await ContactDbContext.ContactItems.FindAsync(id);
        if (item is null) { throw new("Can't GetById: contact with given Id don't exist"); }
        return item;
    }

    public async Task<Contact> Create(Contact item)
    {
        var exists = await ContactDbContext.ContactItems.FindAsync(item.Id);
        if (exists is not null) { throw new("Can't Create: contact with given Id already exists"); }

        ContactDbContext.ContactItems.Add(item);
        await ContactDbContext.SaveChangesAsync();
        return item;
    }

    public async Task<Contact> Update(Contact item)
    {
        var exists = await ContactDbContext.ContactItems.FindAsync(item.Id);
        if (exists is null) { throw new("Can't Update: contact with given Id don't exist"); }

        ContactDbContext.ContactItems.Remove(exists);
        ContactDbContext.ContactItems.Add(item);
        await ContactDbContext.SaveChangesAsync();
        return item;
    }

    public async Task<Contact> DeleteById(int id)
    {
        var item = await ContactDbContext.ContactItems.FindAsync(id);
        if (item is null) { throw new("Can't Delete: contact with given Id don't exist"); }

        ContactDbContext.ContactItems.Remove(item);
        await ContactDbContext.SaveChangesAsync();

        return item;
    }
}
