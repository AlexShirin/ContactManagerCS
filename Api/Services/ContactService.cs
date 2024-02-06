using ContactManagerCS.Exceptions;
using AutoMapper;
using FluentValidation;
using ContactManagerCS.Services.Models;
using ContactManagerCS.Services.Validators;
using ContactManagerCS.DAL.Repositories;
using ContactManagerCS.DAL.Models;

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

    public async Task<List<GetAllContactResponse>> GetAll()
    {
        var contacts = await _contactRepository.GetAll();
        return _mapper.Map<List<GetAllContactResponse>>(contacts);
    }

    public async Task<GetByIdContactResponse> GetById(int id)
    {
        var contact = await _contactRepository.GetById(id);
        if (contact is null)
        {
            throw new ContactException("Can't GetById: contact with given Id don't exist");
        }
        return _mapper.Map<GetByIdContactResponse>(contact);
    }

    public async Task<CreateContactResponse> Create(CreateContactRequest create)
    {
        CreateContactRequestValidator validator = new();
        await validator.ValidateAndThrowAsync(create);

        var contact = _mapper.Map<Contact>(create);

        var exists = await _contactRepository.GetById(contact.Id);
        if (exists is not null) 
        { 
            throw new ContactException("Can't Create: contact with given Id already exists"); 
        }

        await _contactRepository.Create(contact);

        return _mapper.Map<CreateContactResponse>(contact);
    }

    public async Task<List<FindContactResponse>> Find(FindContactRequest find)
    {
        FindContactRequestValidator validator = new();
        await validator.ValidateAndThrowAsync(find);

        var found = await _contactRepository.Find(find.Keyword);

        return _mapper.Map<List<FindContactResponse>>(found);
    }

    public async Task<UpdateContactResponse> Update(UpdateContactRequest update)
    {
        UpdateContactRequestValidator validator = new();
        await validator.ValidateAndThrowAsync(update);

        var contact = _mapper.Map<Contact>(update);

        _ = await _contactRepository.GetById(contact.Id) ??
            throw new ContactException("Can't Update: contact with given Id don't exist");

        await _contactRepository.Update(contact);

        return _mapper.Map<UpdateContactResponse>(contact);
    }

    public async Task<DeleteContactResponse> DeleteById(int id)
    {
        if (id < 1) throw new ContactException($"Can't Delete: id = {id} isn't possible, id must be positive integer");

        var item = await _contactRepository.GetById(id) ?? 
            throw new ContactException("Can't Delete: contact with given Id don't exist");
        await _contactRepository.Delete(item);

        return _mapper.Map<DeleteContactResponse>(item);
    }
}
