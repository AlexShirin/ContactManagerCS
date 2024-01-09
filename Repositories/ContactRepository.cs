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

public class ContactRepository(ContactDbContext contactDbContext, IMapper mapper) : IContactRepository
{
    public async Task<List<ContactResponse>> GetAll()
    {
        var contacts = await contactDbContext.ContactItems.ToListAsync();
        return mapper.Map<List<ContactResponse>>(contacts);
    }

    public async Task<ContactResponse> GetById(int id)
    {
        var contact = await contactDbContext.ContactItems.FindAsync(id);
        if (contact is null) { throw new("Can't GetById: contact with given Id don't exist"); }
        return mapper.Map<ContactResponse>(contact);
    }

    public async Task<ContactResponse> Create(AddContactRequest item)
    {
        AddContactRequestValidator validator = new();
        validator.ValidateAndThrow(item);

        var contact = mapper.Map<Contact>(item);

        var exists = await contactDbContext.ContactItems.FindAsync(contact.Id);
        if (exists is not null) { throw new("Can't Create: contact with given Id already exists"); }

        contactDbContext.ContactItems.Add(contact);
        await contactDbContext.SaveChangesAsync();

        return mapper.Map<ContactResponse>(contact);
    }

    public async Task<ContactResponse> Update(AddContactRequest item)
    {
        AddContactRequestValidator validator = new();
        validator.ValidateAndThrow(item);

        var contact = mapper.Map<Contact>(item);

        var exists = await contactDbContext.ContactItems.FindAsync(contact.Id);
        if (exists is null) { throw new("Can't Update: contact with given Id don't exist"); }

        contactDbContext.ContactItems.Remove(exists);
        contactDbContext.ContactItems.Add(contact);
        await contactDbContext.SaveChangesAsync();

        return mapper.Map<ContactResponse>(contact);
    }

    public async Task<ContactResponse> DeleteById(int id)
    {
        var item = await contactDbContext.ContactItems.FindAsync(id);
        if (item is null) { throw new("Can't Delete: contact with given Id don't exist"); }

        contactDbContext.ContactItems.Remove(item);
        await contactDbContext.SaveChangesAsync();
        return mapper.Map<ContactResponse>(item);
    }
}
