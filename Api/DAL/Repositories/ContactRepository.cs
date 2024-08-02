using System.Xml.Linq;
using ContactManagerCS.DAL.Database;
using ContactManagerCS.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.DAL.Repositories;

public class ContactRepository : IContactRepository
{
    protected readonly DbContext Context;
    protected readonly DbSet<Contact> DbSet;

    public ContactRepository(ContactContext context)
    {
        Context = context;
        DbSet = context.Set<Contact>();
    }

    public async Task<List<Contact>> GetAll()
    {
        return await DbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Contact?> GetById(int id)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Contact> Create(Contact contact)
    {
        DbSet.Add(contact);
        await Context.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> Delete(Contact contact)
    {
        DbSet.Remove(contact);
        await Context.SaveChangesAsync();
        return contact;
    }

    public async Task<List<Contact>> Find(string keyword)
    {
        var found = await DbSet
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
        DbSet.Update(contact);
        await Context.SaveChangesAsync();
        return contact;
    }
}
