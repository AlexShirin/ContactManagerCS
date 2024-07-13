using AutoMapper;
using ContactManagerCS.Common.Exceptions;
using ContactManagerCS.Common.Loggers;
using ContactManagerCS.DAL.Entities;
using ContactManagerCS.DAL.Repositories;
using ContactManagerCS.Services;
using ContactManagerCS.Services.Models;
using ContactManagerCS.Tests.Helpers;
using FluentValidation;
using Moq;

namespace ContactManagerCS.Tests;

public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _mockRepo;
    private readonly ContactService _contactService;
    //private readonly RabbitMQLogger _logger;
    private readonly IMapper _mapper;

    public ContactServiceTests()
    {
        var contactMapperProfile = new ContactMapper();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(contactMapperProfile));
        _mapper = new Mapper(mapperConfiguration);
        //_logger = new RabbitMQLogger(new());
        _mockRepo = new Mock<IContactRepository>();
        _contactService = new ContactService(_mockRepo.Object, null, _mapper);
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
        Assert.IsType<List<GetAllContactResponse>>(result);
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
        Assert.IsType<GetByIdContactResponse>(result);
        Assert.Equal(contact.Id, result.Id);
        Assert.Equal(contact.Name, result.Name);
        Assert.Equal(contact.Email, result.Email);
        Assert.Equal(contact.Phone, result.Phone);
        Assert.Equal(contact.Company, result.Company);
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
    [InlineData(null, "e@e.e", "7", "", false, "Please specify a non-empty name")]
    [InlineData("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a", "e@e.e", "", "", false, "Name length is 100 symbols max")]
    [InlineData("n", null, "7", "", false, "Please specify a non-empty email")]
    [InlineData("n", "e", "7", "", false, "Email must be valid e-mail address")]
    [InlineData("n", "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890e@e.e", "7", "", false, "Email length is 100 symbols max")]
    [InlineData("n", "e@e.e", null, "", false, "Please specify a non-empty phone")]
    [InlineData("n", "e@e.e", "p", "", false, "Phone must contain only digits [0-9]")]
    [InlineData("n", "e@e.e", "123456789012345678901234567890123", "", false, "Phone length is 32 digits max")]
    [InlineData("n", "e@e.e", "7", "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a", false, "Company length is 100 symbols max")]
    [InlineData("Jim", "d@d.d", "777", "D", true, "")]
    public async Task CreateValidationFailWithExceptionAndSuccessfulCreate(
        string name, string email, string phone, string company, bool valid, string errorMessage)
    {
        //Arrange
        var createContactRequest = new CreateContactRequest { Name = name, Email = email, Phone = phone, Company = company };
        var createContact = new Contact { Name = name, Email = email, Phone = phone, Company = company };
        CreateContactResponse result = null;
        Exception? ex = null;

        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Contact)null);
        _mockRepo.Setup(repo => repo.Create(createContact)).ReturnsAsync(createContact);

        //Act
        if (!valid)
        {
            ex = await Assert.ThrowsAsync<ValidationException>(() => _contactService.Create(createContactRequest));
        }
        else
        {
            result = await _contactService.Create(createContactRequest);
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
            Assert.IsType<CreateContactResponse>(result);
            Assert.Equal(createContactRequest.Name, result.Name);
            Assert.Equal(createContactRequest.Email, result.Email);
            Assert.Equal(createContactRequest.Phone, result.Phone);
            Assert.Equal(createContactRequest.Company, result.Company);
        }
    }

    [Fact]
    public async Task CreateExistingContact_ReturnException()
    {
        //Arrange
        var contact = ContactHelper.BaseContactList.ElementAt(0);
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(contact);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(
            () => _contactService.Create(ContactHelper.ContactToCreate));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<ContactException>(exception);
        Assert.Contains("Can't Create: contact with given Id already exists", exception.Message);
    }

    [Theory]
    [InlineData(0, "n", "e@e.e", "7", "", false, "Id must be positive integer")]
    [InlineData(1, null, "e@e.e", "7", "", false, "Please specify a non-empty name")]
    [InlineData(1, "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a", "e@e.e", "", "", false, "Name length is 100 symbols max")]
    [InlineData(1, "n", null, "7", "", false, "Please specify a non-empty email")]
    [InlineData(1, "n", "@.", "7", "", false, "Email must be valid e-mail address")]
    [InlineData(1, "n", "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890e@e.e", "7", "", false, "Email length is 100 symbols max")]
    [InlineData(1, "n", "e@e.e", null, "", false, "Please specify a non-empty phone")]
    [InlineData(1, "n", "e@e.e", "p", "", false, "Phone must contain only digits [0-9]")]
    [InlineData(1, "n", "e@e.e", "123456789012345678901234567890123", "", false, "Phone length is 32 digits max")]
    [InlineData(1, "n", "e@e.e", "7", "1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a", false, "Company length is 100 symbols max")]
    [InlineData(4, "Jim", "d@d.d", "777", "D", true, "")]
    public async Task UpdateValidationFailWithExceptionAndSuccessfulUpdate(
        int id, string name, string email, string phone, string work, bool valid, string errorMessage)
    {
        //Arrange
        var updateContactRequest = new UpdateContactRequest { Id = id, Name = name, Email = email, Phone = phone, Company = work };
        var updateContact = new Contact { Id = id, Name = name, Email = email, Phone = phone, Company = work };
        UpdateContactResponse? result = null;
        Exception? ex = null;

        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(updateContact);
        _mockRepo.Setup(repo => repo.Update(updateContact)).ReturnsAsync(updateContact);

        //Act
        if (!valid)
        {
            ex = await Assert.ThrowsAsync<ValidationException>(() => _contactService.Update(updateContactRequest));
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
            Assert.IsType<UpdateContactResponse>(result);
            Assert.Equal(updateContact.Id, result.Id);
            Assert.Equal(updateContact.Name, result.Name);
            Assert.Equal(updateContact.Email, result.Email);
            Assert.Equal(updateContact.Phone, result.Phone);
            Assert.Equal(updateContact.Company, result.Company);
        }
    }

    [Fact]
    public async Task UpdateNotExistContact_ReturnException()
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Contact)null);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(
            () => _contactService.Update(ContactHelper.ContactToUpdate));

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
        _mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync(contact);
        _mockRepo.Setup(repo => repo.Delete(contact)).ReturnsAsync(contact);

        //Act
        var result = await _contactService.DeleteById(1);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<DeleteContactResponse>(result);
        Assert.Equal(contact.Id, result.Id);
        Assert.Equal(contact.Name, result.Name);
        Assert.Equal(contact.Email, result.Email);
        Assert.Equal(contact.Phone, result.Phone);
        Assert.Equal(contact.Company, result.Company);
    }

    [Fact]
    public async Task DeleteNotExistContact_ReturnException()
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(1)).ReturnsAsync((Contact)null);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(
            () => _contactService.DeleteById(1));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<ContactException>(exception);
        Assert.Contains("Can't Delete: contact with given Id don't exist", exception.Message);
    }

    [Theory]
    [InlineData(null, false, "Please specify a non-empty Keyword")]
    [InlineData("a", true, "")]
    public async Task FindValidationFailWithExceptionAndSuccessfulFind(string keyword, bool valid, string errorMessage)
    {
        //Arrange
        var findContactRequest = new FindContactRequest { Keyword = keyword };
        List<FindContactResponse>? result = null;
        Exception? ex = null;

        _mockRepo.Setup(repo => repo.Find(It.IsAny<string>())).ReturnsAsync(ContactHelper.BaseContactList);

        //Act
        if (!valid)
        {
            ex = await Assert.ThrowsAsync<ValidationException>(() => _contactService.Find(findContactRequest));
        }
        else
        {
            result = await _contactService.Find(findContactRequest);
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
            Assert.IsType<List<FindContactResponse>>(result);
            Assert.Equal(ContactHelper.BaseContactList.Count, result.Count);
        }
    }
}