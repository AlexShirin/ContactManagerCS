using ContactManagerCS.Contracts;
using ContactManagerCS.Exceptions;
using ContactManagerCS.Models;
using ContactManagerCS.Validation;
using AutoMapper;
using FluentValidation;

namespace ContactManagerCS.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;

    public ContactService(IContactRepository contactRepository, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    public async Task<List<ContactResponse>> GetAll()
    {
        var contacts = await _contactRepository.GetAll();
        return _mapper.Map<List<ContactResponse>>(contacts);
    }

    public async Task<ContactResponse> GetById(int id)
    {
        var contact = await _contactRepository.GetById(id);
        if (contact is null)
        {
            throw new ContactException("Can't GetById: contact with given Id don't exist");
        }
        return _mapper.Map<ContactResponse>(contact);
    }

    public async Task<ContactResponse> Create(AddContactRequest item)
    {
        AddContactRequestValidator validator = new();
        validator.ValidateAndThrow(item);

        var contact = _mapper.Map<Contact>(item);

        var exists = await _contactRepository.GetById(contact.Id);
        if (exists is not null) 
        { 
            throw new ContactException("Can't Create: contact with given Id already exists"); 
        }

        await _contactRepository.Create(contact);

        return _mapper.Map<ContactResponse>(contact);
    }

    public async Task<ContactResponse> Update(AddContactRequest item)
    {
        AddContactRequestValidator validator = new();
        validator.ValidateAndThrow(item);

        var contact = _mapper.Map<Contact>(item);

        var exists = await _contactRepository.GetById(contact.Id) ??
            throw new ContactException("Can't Update: contact with given Id don't exist"); 

        await _contactRepository.Update(exists, contact);

        return _mapper.Map<ContactResponse>(contact);
    }

    public async Task<ContactResponse> DeleteById(int id)
    {
        var item = await _contactRepository.GetById(id) ?? 
            throw new ContactException("Can't Delete: contact with given Id don't exist");
        await _contactRepository.Delete(item);

        return _mapper.Map<ContactResponse>(item);
    }
}
