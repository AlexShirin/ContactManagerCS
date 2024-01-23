using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using ContactManagerCS.Contracts;
using ContactManagerCS.Exceptions;
using ContactManagerCS.Models;
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

    //private List<Contact> _listContacts;
    //private List<ContactResponse> _listContactResponses;

    //private int _id;
    //private Contact _contact4;
    //private AddContactRequest _contact4req;
    //private ContactResponse _contact4res;
    //private Contact _contact4u;
    //private AddContactRequest _contact4ureq;
    //private ContactResponse _contact4ures;

    public ContactServiceTests()
    {
        var contactMapperProfile = new ContactMapper();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(contactMapperProfile));
        _mapper = new Mapper(mapperConfiguration);
        _mockRepo = new Mock<IContactRepository>();
        _contactService = new ContactService(_mockRepo.Object, _mapper);

        //_listContacts =
        //[
        //    new Contact { Id = 1, Name = "Tom", Email = "a@a.a", Phone = "11", Work = "A" },
        //    new Contact { Id = 2, Name = "Bob", Email = "b@a.a", Phone = "22", Work = "B" },
        //    new Contact { Id = 3, Name = "Sam", Email = "c@a.a", Phone = "33", Work = "C" },
        //];

        //_listContactResponses =
        //[
        //    new ContactResponse { Id = 1, Name = "Tom", Email = "a@a.a", Phone = "11", Work = "A" },
        //    new ContactResponse { Id = 2, Name = "Bob", Email = "b@a.a", Phone = "22", Work = "B" },
        //    new ContactResponse { Id = 3, Name = "Sam", Email = "c@a.a", Phone = "33", Work = "C" },
        //];

        //_id = 4;
        //_contact4 = new Contact { Id = 4, Name = "d", Email = "d@d.d", Phone = "44", Work = "D" };
        //_contact4req = new AddContactRequest { Id = 4, Name = "d", Email = "d@d.d", Phone = "44", Work = "D" };
        //_contact4res = new ContactResponse { Id = 4, Name = "d", Email = "d@d.d", Phone = "44", Work = "D" };
        //_contact4u = new Contact { Id = 4, Name = "e", Email = "d@d.d", Phone = "44", Work = "D" };
        //_contact4ureq = new AddContactRequest { Id = 4, Name = "e", Email = "d@d.d", Phone = "44", Work = "D" };
        //_contact4ures = new ContactResponse { Id = 4, Name = "e", Email = "d@d.d", Phone = "44", Work = "D" };
    }

    [Fact]
    public async Task GetAllTest()
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetAll().Result).Returns(ContactHelper.BaseContactList);

        //Act
        var result = await _contactService.GetAll();

        //Assert
        Assert.NotNull(result);
        Assert.IsType<List<ContactResponse>>(result);
        Assert.Equal(ContactHelper.BaseContactList.Count, result.Count);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetByIdValidTest(int id)
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(id)).ReturnsAsync(ContactHelper.BaseContactList.ElementAt(id));

        //Act
        var result = await _contactService.GetById(id);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ContactResponse>(result);
        Assert.Equal(ContactHelper.BaseContactResponseList.ElementAt(id), result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(3)]
    public async Task GetByIdNotValidTest(int id)
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(id)).ReturnsAsync((Contact)null);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(() => _contactService.GetById(id));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<ContactException>(exception);
        Assert.Contains("Can't GetById: contact with given Id don't exist", exception.Message);
    }

    [Theory]
    [InlineData(null, "", "", "", "", false, false)]
    [InlineData(1, null, "", "", "", false, false)]
    [InlineData(1, "", null, "", "", false, false)]
    [InlineData(1, "", "", null, "", false, false)]
    [InlineData(1, "", "", "", null, false, false)]
    [InlineData(2, "Jim", "d@d.d", "44", "D", true, true)]
    [InlineData(3, "Jim", "d@d.d", "44", "D", true, false)]
    public async Task CreateTest(int? id, string name, string email, string phone, string work, bool valid, bool exists)
    {
        //Arrange
        var addContactRequest = new AddContactRequest { 
            Id = id, Name = name, Email = email, Phone = phone, Work = work };
        var addContact = new Contact { 
            Id = (id == null ? 0 : id.Value), Name = name, Email = email, Phone = phone, Work = work };
        var addContactResponse = new ContactResponse { 
            Id = (id == null ? 0 : id.Value), Name = name, Email = email, Phone = phone, Work = work };
        ContactResponse response = null;
        ContactException exception = null;
        //ValidationException validation = null;
        Exception ex = null;

        if (exists) 
        {
            _mockRepo.Setup(repo => repo.GetById(addContact.Id).Result).Returns(addContact);
        }
        else
        {
            _mockRepo.Setup(repo => repo.GetById(addContact.Id)).ReturnsAsync((Contact)null);
        }
        _mockRepo.Setup(repo => repo.Create(addContact).Result).Returns(addContact);

        //Act
        if (!valid)
        {
            ex = await Assert.ThrowsAsync<ValidationException>(() => _contactService.Create(addContactRequest));
        }
        else if (exists)
        {
            exception = await Assert.ThrowsAsync<ContactException>(() => _contactService.Create(addContactRequest));
        }
        else
        {
            response = await _contactService.Create(addContactRequest);
        }

        //Assert
        if (!valid)
        {
            Assert.NotNull(ex);
            Assert.IsType<ValidationException>(ex);
            if (id == null)
            {
                Assert.Contains("Please specify a unique id", ex.Message);
            }
            else if (name == null)
            {
                Assert.Contains("Please specify a non-empty name", ex.Message);
            }
            else if (email == null)
            {
                Assert.Contains("Please specify a non-empty email", ex.Message);
            }
            else if (phone == null)
            {
                Assert.Contains("Please specify a non-empty phone", ex.Message);
            }
            else if (work == null)
            {
                Assert.Contains("Please specify a non-empty work name", ex.Message);
            }
        }
        else if(exists)
        {
            Assert.NotNull(exception);
            Assert.IsType<ContactException>(exception);
            Assert.Contains("Can't Create: contact with given Id already exists", exception.Message);
        }
        else
        {
            Assert.NotNull(response);
            Assert.IsType<ContactResponse>(response);
            Assert.Equal(addContactResponse, response);
        }
    }

    [Theory]
    [MemberData(nameof(ContactHelper.CreateValidTestData), MemberType = typeof(ContactHelper))]
    public async Task CreateValidTest(Contact contact)
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(contact.Id)).ReturnsAsync((Contact)null);
        _mockRepo.Setup(repo => repo.Create(contact).Result).Returns(contact);

        //Act
        var result1 = await _contactService.Create(new AddContactRequest(contact));

        //Assert
        Assert.NotNull(result1);
        Assert.IsType<ContactResponse>(result1);
        Assert.Equal(new ContactResponse(contact), result1);
    }

    [Theory]
    [MemberData(nameof(ContactHelper.CreateNotValidTestData), MemberType = typeof(ContactHelper))]
    public async Task CreateNotValidTest(Contact contact)
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(contact.Id).Result).Returns(contact);
        _mockRepo.Setup(repo => repo.Create(contact).Result).Returns(contact);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(
            () => _contactService.Create(new AddContactRequest(contact)));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<ContactException>(exception);
        Assert.Contains("Can't Create: contact with given Id already exists", exception.Message);
    }

    [Theory]
    [MemberData(nameof(ContactHelper.UpdateValidTestData), MemberType = typeof(ContactHelper))]
    public async Task UpdateValidTest(Contact contactToAdd, Contact contactToUpdate)
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(contactToAdd.Id).Result).Returns(contactToAdd);
        _mockRepo.Setup(repo => repo.Update(contactToAdd, contactToUpdate).Result).Returns(contactToUpdate);

        //Act
        var result2 = await _contactService.Update(new AddContactRequest(contactToUpdate));

        //Assert
        Assert.NotNull(result2);
        Assert.IsType<ContactResponse>(result2);
        Assert.Equal(new ContactResponse(contactToUpdate), result2);
    }

    [Theory]
    [MemberData(nameof(ContactHelper.UpdateNotValidTestData), MemberType = typeof(ContactHelper))]
    public async Task UpdateNotValidTest(Contact contactToAdd, Contact contactToUpdate)
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(contactToAdd.Id).Result).Returns((Contact)null);
        _mockRepo.Setup(repo => repo.Update(contactToAdd, contactToUpdate).Result).Returns(contactToUpdate);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(
            () => _contactService.Update(new AddContactRequest(contactToUpdate)));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<ContactException>(exception);
        Assert.Contains("Can't Update: contact with given Id don't exist", exception.Message);
    }

    [Theory]
    [MemberData(nameof(ContactHelper.DeleteValidTestData), MemberType = typeof(ContactHelper))]
    public async Task DeleteValidTest(Contact contactToUpdate)
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(contactToUpdate.Id).Result).Returns(contactToUpdate);
        _mockRepo.Setup(repo => repo.Delete(contactToUpdate).Result).Returns(contactToUpdate);

        //Act
        var result3 = await _contactService.DeleteById(contactToUpdate.Id);

        //Assert
        Assert.NotNull(result3);
        Assert.IsType<ContactResponse>(result3);
        Assert.Equal(new ContactResponse(contactToUpdate), result3);
    }

    [Theory]
    [MemberData(nameof(ContactHelper.DeleteNotValidTestData), MemberType = typeof(ContactHelper))]
    public async Task DeleteNotValidTest(Contact contactToUpdate)
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(contactToUpdate.Id).Result).Returns((Contact)null);
        _mockRepo.Setup(repo => repo.Delete(contactToUpdate).Result).Returns(contactToUpdate);

        //Act
        var exception = await Assert.ThrowsAsync<ContactException>(
            () => _contactService.DeleteById(contactToUpdate.Id));

        //Assert
        Assert.NotNull(exception);
        Assert.IsType<ContactException>(exception);
        Assert.Contains("Can't Delete: contact with given Id don't exist", exception.Message);
    }
}