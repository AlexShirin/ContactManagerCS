using AutoMapper;

using ContactManagerCS.Contracts;
using ContactManagerCS.Database;
using ContactManagerCS.Models;
using ContactManagerCS.Validation;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly ContactDbContext contactDbContext;
    private readonly IMapper mapper;

    public ContactRepository(ContactDbContext contactDbContext, IMapper mapper)
    {
        this.contactDbContext = contactDbContext;
        this.mapper = mapper;
    }

    public async Task<List<Contact>> GetAll()
    {
        return await contactDbContext.ContactItems.ToListAsync();
    }

    public async Task<Contact?> GetById(int id)
    {
        return await contactDbContext.ContactItems.FindAsync(id);
    }

    public async Task<Contact> Create(Contact contact)
    {
        contactDbContext.ContactItems.Add(contact);
        await contactDbContext.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> Update(Contact exists, Contact contact)
    {
        exists.Id = contact.Id;
        exists.Name = contact.Name;
        exists.Email = contact.Email;
        exists.Phone = contact.Phone;
        exists.Work = contact.Work;

        await contactDbContext.SaveChangesAsync();
        return contact;
    }

    public async Task<Contact> Delete(Contact contact)
    {
        contactDbContext.ContactItems.Remove(contact);
        await contactDbContext.SaveChangesAsync();
        return contact;
    }
}
