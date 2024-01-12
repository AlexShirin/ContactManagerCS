using AutoMapper;

using ContactManagerCS.Contracts;
using ContactManagerCS.Database;
using ContactManagerCS.Models;
using ContactManagerCS.Repositories;
using ContactManagerCS.Validation;

using FluentValidation;

namespace ContactManagerCS.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository contactRepository;
    private readonly IMapper mapper;

    public ContactService(IContactRepository contactRepository, IMapper mapper)
    {
        this.contactRepository = contactRepository;
        this.mapper = mapper;
    }

    public async Task<List<ContactResponse>> GetAll()
    {
        var contacts = await contactRepository.GetAll();
        return mapper.Map<List<ContactResponse>>(contacts);
    }

    public async Task<ContactResponse> GetById(int id)
    {
        var contact = await contactRepository.GetById(id);
        if (contact is null) { throw new("Can't GetById: contact with given Id don't exist"); }
        return mapper.Map<ContactResponse>(contact);
    }

    public async Task<ContactResponse> Create(AddContactRequest item)
    {
        AddContactRequestValidator validator = new();
        validator.ValidateAndThrow(item);

        var contact = mapper.Map<Contact>(item);

        var exists = await contactRepository.GetById(contact.Id);
        if (exists is not null) { throw new("Can't Create: contact with given Id already exists"); }

        await contactRepository.Create(contact);

        return mapper.Map<ContactResponse>(contact);
    }

    public async Task<ContactResponse> Update(AddContactRequest item)
    {
        AddContactRequestValidator validator = new();
        validator.ValidateAndThrow(item);

        var contact = mapper.Map<Contact>(item);

        var exists = await contactRepository.GetById(contact.Id);
        if (exists is null) { throw new("Can't Update: contact with given Id don't exist"); }

        await contactRepository.Update(exists, contact);

        return mapper.Map<ContactResponse>(contact);
    }

    public async Task<ContactResponse> DeleteById(int id)
    {
        var item = await contactRepository.GetById(id);
        if (item is null) { throw new("Can't Delete: contact with given Id don't exist"); }

        await contactRepository.Delete(item);

        return mapper.Map<ContactResponse>(item);
    }
}
