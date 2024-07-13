using AutoMapper;
using FluentValidation;
using ContactManagerCS.Services.Models;
using ContactManagerCS.Services.Validators;
using ContactManagerCS.DAL.Repositories;
using ContactManagerCS.Common.Exceptions;
using ContactManagerCS.DAL.Entities;
using ContactManagerCS.Common.Loggers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ContactManagerCS.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _contactRepository;
    private readonly RabbitMQLogger _logger;
    private readonly IMapper _mapper;

    public ContactService(IContactRepository contactRepository, RabbitMQLogger logger, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<GetAllContactResponse>> GetAll()
    {
        _logger.Log($"Request: GetAll - begin");
        var contacts = await _contactRepository.GetAll();
        _logger.Log($"Response: GetAll - {JsonConvert.SerializeObject(contacts)}");
        return _mapper.Map<List<GetAllContactResponse>>(contacts);
    }

    public async Task<GetByIdContactResponse> GetById(int id)
    {
        _logger.Log($"Request: GetById({JsonConvert.SerializeObject(id)})");
        var contact = await _contactRepository.GetById(id);
        _logger.Log($"Response: GetById({JsonConvert.SerializeObject(contact)})");
        if (contact is null)
        {
            throw new ContactException($"Can't GetById: contact with given Id = {id} don't exist");
        }
        return _mapper.Map<GetByIdContactResponse>(contact);
    }

    public async Task<CreateContactResponse> Create(CreateContactRequest create)
    {
        _logger.Log($"Request: Create({JsonConvert.SerializeObject(create)})");
        CreateContactRequestValidator validator = new();
        await validator.ValidateAndThrowAsync(create);

        var contact = _mapper.Map<Contact>(create);

        var exists = await _contactRepository.GetById(contact.Id);
        if (exists is not null) 
        { 
            throw new ContactException("Can't Create: contact with given Id already exists"); 
        }

        await _contactRepository.Create(contact);
        _logger.Log($"Response: Create({JsonConvert.SerializeObject(contact)})");

        return _mapper.Map<CreateContactResponse>(contact);
    }

    public async Task<List<FindContactResponse>> Find(FindContactRequest find)
    {
        _logger.Log($"Request: Find({JsonConvert.SerializeObject(find)})");
        FindContactRequestValidator validator = new();
        await validator.ValidateAndThrowAsync(find);

        var found = await _contactRepository.Find(find.Keyword);
        _logger.Log($"Response: Find({JsonConvert.SerializeObject(found)})");

        return _mapper.Map<List<FindContactResponse>>(found);
    }

    public async Task<UpdateContactResponse> Update(UpdateContactRequest update)
    {
        _logger.Log($"Request: Update({JsonConvert.SerializeObject(update)})");
        UpdateContactRequestValidator validator = new();
        await validator.ValidateAndThrowAsync(update);

        var contact = _mapper.Map<Contact>(update);

        _ = await _contactRepository.GetById(contact.Id) ??
            throw new ContactException("Can't Update: contact with given Id don't exist");

        await _contactRepository.Update(contact);
        _logger.Log($"Response: Update({JsonConvert.SerializeObject(contact)})");

        return _mapper.Map<UpdateContactResponse>(contact);
    }

    public async Task<DeleteContactResponse> DeleteById(int id)
    {
        _logger.Log($"Request: DeleteById({JsonConvert.SerializeObject(id)})");
        if (id < 1) throw new ContactException($"Can't Delete: id = {id} isn't possible, id must be positive integer");

        var item = await _contactRepository.GetById(id) ?? 
            throw new ContactException("Can't Delete: contact with given Id don't exist");
        await _contactRepository.Delete(item);
        _logger.Log($"Response: DeleteById({JsonConvert.SerializeObject(item)})");

        return _mapper.Map<DeleteContactResponse>(item);
    }
}
