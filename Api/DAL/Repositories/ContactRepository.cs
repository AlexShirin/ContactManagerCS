using System.Xml.Linq;

using ContactManagerCS.DAL.Database;
using ContactManagerCS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.DAL.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly ContactContext _contactContext;

    public ContactRepository(ContactContext contactDbContext)
    {
        _contactContext = contactDbContext;
    }

    public async Task<List<Contact>> GetAll()
    {
        return await _contactContext.Contact
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Contact?> GetById(int id)
    {
        return await _contactContext.Contact
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Contact> Create(Contact contact)
    {
        _contactContext.Contact.Add(contact);
        await _contactContext.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> Delete(Contact contact)
    {
        _contactContext.Contact.Remove(contact);
        await _contactContext.SaveChangesAsync();
        return contact;
    }

    public async Task<List<Contact>> Find(string keyword)
    {
        var found = await _contactContext.Contact
            .Where(x => x.Name.Contains(keyword)
                || x.Email.Contains(keyword)
                || x.Phone.Contains(keyword)
                || x.Company.Contains(keyword))
            //.Where(x => EF.Functions.ILike(x.Name, keyword)
            //    || EF.Functions.ILike(x.Email, keyword)
            //    || EF.Functions.ILike(x.Phone, keyword)
            //    || EF.Functions.ILike(x.Company, keyword))
            .ToListAsync();
        return found;
    }

    public async Task<Contact> Update(Contact contact)
    {
        _contactContext.Contact.Update(contact);
        await _contactContext.SaveChangesAsync();
        return contact;
    }
}
