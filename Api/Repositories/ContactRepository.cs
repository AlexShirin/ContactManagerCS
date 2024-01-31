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
        return await _contactDbContext.ContactItems
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Contact?> GetById(int id)
    {
        return await _contactDbContext.ContactItems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Contact> Create(Contact contact)
    {
        _contactDbContext.ContactItems.Add(contact);
        await _contactDbContext.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> Delete(Contact contact)
    {
        _contactDbContext.ContactItems.Remove(contact);
        await _contactDbContext.SaveChangesAsync();
        return contact;
    }

    public async Task<List<Contact>> Find(string keyword)
    {
        var found = await _contactDbContext.ContactItems
            .Where(x => x.Name.Contains(keyword)
                || x.Email.Contains(keyword)
                || x.Phone.Contains(keyword)
                || x.Company.Contains(keyword))
            .ToListAsync();
        return found;
    }

    public async Task<Contact> Update(Contact contact)
    {
        _contactDbContext.ContactItems.Update(contact);
        await _contactDbContext.SaveChangesAsync();
        return contact;
    }
}
