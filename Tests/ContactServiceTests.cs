using AutoMapper;
using ContactManagerCS.Common.Exceptions;
using ContactManagerCS.DAL.Models;
using ContactManagerCS.DAL.Repositories;
using ContactManagerCS.Services;
using ContactManagerCS.Tests.Helpers;

using FluentValidation;

using Moq;

namespace ContactManagerCS.Tests;

public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _mockRepo;
    private readonly ContactService _contactService;
    private readonly IMapper _mapper;

    public ContactServiceTests()
    {
        var contactMapperProfile = new ContactMapper();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(contactMapperProfile));
        _mapper = new Mapper(mapperConfiguration);
        _mockRepo = new Mock<IContactRepository>();
        _contactService = new ContactService(_mockRepo.Object, _mapper);
    }

    [Fact]
    public async Task GetAllContacts_Success()
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(ContactHelper.BaseContactList);

        //Act
        var result = await _contactService.GetAll();

        //Assert
        Assert.NotNull(result);
        Assert.IsType<List<ContactResponse>>(result);
        Assert.Equal(ContactHelper.BaseContactList.Count, result.Count);
    }

    [Fact]
    public async Task GetByIdContactExist_Success()
    {
        //Arrange
        var contact = ContactHelper.BaseContactList.ElementAt(0);
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(contact);

        //Act
        var result = await _contactService.GetById(It.IsAny<int>());

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ContactResponse>(result);
        Assert.Equal(contact.Id, result.Id);
        Assert.Equal(contact.Name, result.Name);
        Assert.Equal(contact.Email, result.Email);
        Assert.Equal(contact.Phone, result.Phone);
        Assert.Equal(contact.Work, result.Work);
    }

    [Fact]
    public async Task GetByIdContactNotExist_ReturnException()
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Contact)null);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(() => _contactService.GetById(It.IsAny<int>()));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<ContactException>(exception);
        Assert.Contains("Can't GetById: contact with given Id don't exist", exception.Message);
    }

    [Theory]
    [InlineData(null, "", "", "", "", false, "Please specify a unique id")]
    [InlineData(1, null, "", "", "", false, "Please specify a non-empty name")]
    [InlineData(1, "", null, "", "", false, "Please specify a non-empty email")]
    [InlineData(1, "", "", null, "", false, "Please specify a non-empty phone")]
    [InlineData(1, "", "", "", null, false, "Please specify a non-empty work name")]
    [InlineData(3, "Jim", "d@d.d", "44", "D", true, "")]
    public async Task CreateValidationFailWithExceptionAndSuccessfulCreate(
        int? id, string name, string email, string phone, string work, bool valid, string errorMessage)
    {
        //Arrange
        var addContactRequest = new AddContactRequest { Id = (id == null ? 0 : id.Value), Name = name, Email = email, Phone = phone, Work = work };
        var addContact = new Contact { Id = (id == null ? 0 : id.Value), Name = name, Email = email, Phone = phone, Work = work };
        ContactResponse? result = null;
        Exception? ex = null;

        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Contact)null);
        _mockRepo.Setup(repo => repo.Create(addContact)).ReturnsAsync(addContact);

        //Act
        if (!valid)
        {
            ex = await Assert.ThrowsAsync<ValidationException>(() => _contactService.Create(addContactRequest));
        }
        else
        {
            result = await _contactService.Create(addContactRequest);
        }

        //Assert
        if (!valid)
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            Assert.Contains(errorMessage, ex.Message);
        }
        else
        {
            Assert.NotNull(result);
            Assert.IsType<ContactResponse>(result);
            Assert.Equal(addContactRequest.Id, result.Id);
            Assert.Equal(addContactRequest.Name, result.Name);
            Assert.Equal(addContactRequest.Email, result.Email);
            Assert.Equal(addContactRequest.Phone, result.Phone);
            Assert.Equal(addContactRequest.Work, result.Work);
        }
    }

    [Fact]
    public async Task CreateExistingContact_ReturnException()
    {
        //Arrange
        var contact = ContactHelper.BaseContactList.ElementAt(0);
        var addContactRequest = _mapper.Map<AddContactRequest>(contact);
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(contact);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(
            () => _contactService.Create(addContactRequest));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<ContactException>(exception);
        Assert.Contains("Can't Create: contact with given Id already exists", exception.Message);
    }

    [Theory]
    [InlineData(null, "", "", "", "", false, "Please specify a unique id")]
    [InlineData(1, null, "", "", "", false, "Please specify a non-empty name")]
    [InlineData(1, "", null, "", "", false, "Please specify a non-empty email")]
    [InlineData(1, "", "", null, "", false, "Please specify a non-empty phone")]
    [InlineData(1, "", "", "", null, false, "Please specify a non-empty work name")]
    [InlineData(3, "Jim", "d@d.d", "44", "D", true, "")]
    public async Task UpdateValidationFailWithExceptionAndSuccessfulUpdate(
        int? id, string name, string email, string phone, string work, bool valid, string errorMessage)
    {
        //Arrange
        var addContactRequest = new AddContactRequest { Id = (id == null ? 0 : id.Value), Name = name, Email = email, Phone = phone, Work = work };
        var addContact = new Contact { Id = (id == null ? 0 : id.Value), Name = name, Email = email, Phone = phone, Work = work };
        var updateContact = ContactHelper.ContactToUpdate;
        var updateContactRequest = _mapper.Map<AddContactRequest>(updateContact);
        ContactResponse? result = null;
        Exception? ex = null;

        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(addContact);
        _mockRepo.Setup(repo => repo.Update(addContact, updateContact)).ReturnsAsync(updateContact);

        //Act
        if (!valid)
        {
            ex = await Assert.ThrowsAsync<ValidationException>(() => _contactService.Update(addContactRequest));
        }
        else
        {
            result = await _contactService.Update(updateContactRequest);
        }

        //Assert
        if (!valid)
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            Assert.Contains(errorMessage, ex.Message);
        }
        else
        {
            Assert.NotNull(result);
            Assert.IsType<ContactResponse>(result);
            Assert.Equal(updateContact.Id, result.Id);
            Assert.Equal(updateContact.Name, result.Name);
            Assert.Equal(updateContact.Email, result.Email);
            Assert.Equal(updateContact.Phone, result.Phone);
            Assert.Equal(updateContact.Work, result.Work);
        }
    }

    [Fact]
    public async Task UpdateNotExistContact_ReturnException()
    {
        //Arrange
        var addContactRequest = _mapper.Map<AddContactRequest>(ContactHelper.ContactToUpdate);
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Contact)null);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(
            () => _contactService.Update(addContactRequest));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<ContactException>(exception);
        Assert.Contains("Can't Update: contact with given Id don't exist", exception.Message);
    }

    [Fact]
    public async Task DeleteExistsContact_Success()
    {
        //Arrange
        var contact = ContactHelper.BaseContactList.ElementAt(0);
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(contact);
        _mockRepo.Setup(repo => repo.Delete(contact)).ReturnsAsync(contact);

        //Act
        var result = await _contactService.DeleteById(It.IsAny<int>());

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ContactResponse>(result);
        Assert.Equal(contact.Id, result.Id);
        Assert.Equal(contact.Name, result.Name);
        Assert.Equal(contact.Email, result.Email);
        Assert.Equal(contact.Phone, result.Phone);
        Assert.Equal(contact.Work, result.Work);
    }

    [Fact]
    public async Task DeleteNotExistContact_ReturnException()
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Contact)null);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(
            () => _contactService.DeleteById(It.IsAny<int>()));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<ContactException>(exception);
        Assert.Contains("Can't Delete: contact with given Id don't exist", exception.Message);
    }
}