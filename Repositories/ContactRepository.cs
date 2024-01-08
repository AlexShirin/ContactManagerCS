using AutoMapper;

using ContactManagerCS.Contracts;
using ContactManagerCS.Database;
using ContactManagerCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Repositories;

public class ContactRepository(ContactDbContext contactDbContext, IMapper mapper) : IContactRepository
{
    public async Task<List<ContactResponse>> GetAll()
    {
        var contacts = await contactDbContext.ContactItems.ToListAsync();
        return mapper.Map<List<ContactResponse>>(contacts);
    }

    public async Task<Contact> GetById(int id)
    {
        var item = await contactDbContext.ContactItems.FindAsync(id);
        if (item is null) { throw new("Can't GetById: contact with given Id don't exist"); }
        return item;
    }

    public async Task<Contact> Create(Contact item)
    {
        var exists = await contactDbContext.ContactItems.FindAsync(item.Id);
        if (exists is not null) { throw new("Can't Create: contact with given Id already exists"); }

        contactDbContext.ContactItems.Add(item);
        await contactDbContext.SaveChangesAsync();
        return item;
    }

    public async Task<Contact> Update(Contact item)
    {
        var exists = await contactDbContext.ContactItems.FindAsync(item.Id);
        if (exists is null) { throw new("Can't Update: contact with given Id don't exist"); }

        contactDbContext.ContactItems.Remove(exists);
        contactDbContext.ContactItems.Add(item);
        await contactDbContext.SaveChangesAsync();
        return item;
    }

    public async Task<Contact> DeleteById(int id)
    {
        var item = await contactDbContext.ContactItems.FindAsync(id);
        if (item is null) { throw new("Can't Delete: contact with given Id don't exist"); }

        contactDbContext.ContactItems.Remove(item);
        await contactDbContext.SaveChangesAsync();

        return item;
    }
}
