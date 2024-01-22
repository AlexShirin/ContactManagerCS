using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ContactManagerCS.Contracts;
using ContactManagerCS.Exceptions;
using ContactManagerCS.Models;
using ContactManagerCS.Services;
using ContactManagerCS.Tests.Helpers;
using Moq;

namespace ContactManagerCS.Tests;

public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _mockRepo;
    private readonly ContactService _contactService;
    private readonly IMapper _mapper;

    private List<Contact> _listContacts;
    private List<ContactResponse> _listContactResponses;

    private int _id;
    private Contact _contact4;
    private AddContactRequest _contact4req;
    private ContactResponse _contact4res;
    private Contact _contact4u;
    private AddContactRequest _contact4ureq;
    private ContactResponse _contact4ures;

    public ContactServiceTests()
    {
        var contactMapperProfile = new ContactMapper();
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(contactMapperProfile));
        _mapper = new Mapper(mapperConfiguration);
        _mockRepo = new Mock<IContactRepository>();
        _contactService = new ContactService(_mockRepo.Object, _mapper);

        _listContacts =
        [
            new Contact { Id = 1, Name = "Tom", Email = "a@a.a", Phone = "11", Work = "A" },
            new Contact { Id = 2, Name = "Bob", Email = "b@a.a", Phone = "22", Work = "B" },
            new Contact { Id = 3, Name = "Sam", Email = "c@a.a", Phone = "33", Work = "C" },
        ];

        _listContactResponses =
        [
            new ContactResponse { Id = 1, Name = "Tom", Email = "a@a.a", Phone = "11", Work = "A" },
            new ContactResponse { Id = 2, Name = "Bob", Email = "b@a.a", Phone = "22", Work = "B" },
            new ContactResponse { Id = 3, Name = "Sam", Email = "c@a.a", Phone = "33", Work = "C" },
        ];

        _id = 4;
        _contact4 = new Contact { Id = 4, Name = "d", Email = "d@d.d", Phone = "44", Work = "D" };
        _contact4req = new AddContactRequest { Id = 4, Name = "d", Email = "d@d.d", Phone = "44", Work = "D" };
        _contact4res = new ContactResponse { Id = 4, Name = "d", Email = "d@d.d", Phone = "44", Work = "D" };
        _contact4u = new Contact { Id = 4, Name = "e", Email = "d@d.d", Phone = "44", Work = "D" };
        _contact4ureq = new AddContactRequest { Id = 4, Name = "e", Email = "d@d.d", Phone = "44", Work = "D" };
        _contact4ures = new ContactResponse { Id = 4, Name = "e", Email = "d@d.d", Phone = "44", Work = "D" };
    }

    [Theory]
    [MemberData(nameof(ContactTestsHelper.GetAllTestData), MemberType = typeof(ContactTestsHelper))]
    public async Task GetAllTest(List<Contact> listContacts)
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetAll().Result).Returns(listContacts);

        //Act
        var result = await _contactService.GetAll();

        //Assert
        Assert.NotNull(result);
        Assert.IsType<List<ContactResponse>>(result);
        Assert.Equal(listContacts.Count, result.Count);
    }

    [Theory]
    [MemberData(nameof(ContactTestsHelper.GetByIdValidTestData), MemberType = typeof(ContactTestsHelper))]
    public async Task GetByIdValidTest(int id, List<Contact> listContacts)
    {
        //Arrange
        _mockRepo.Setup(repo => repo.GetById(id)).ReturnsAsync(listContacts.ElementAt(id - 1));

        //Act
        var result = await _contactService.GetById(id);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<ContactResponse>(result);
        Assert.Equal(_listContactResponses.ElementAt(id - 1), result);
    }

    [Theory]
    [MemberData(nameof(ContactTestsHelper.GetByIdNotValidTestData), MemberType = typeof(ContactTestsHelper))]
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
    [MemberData(nameof(ContactTestsHelper.CreateValidTestData), MemberType = typeof(ContactTestsHelper))]
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
    [MemberData(nameof(ContactTestsHelper.CreateNotValidTestData), MemberType = typeof(ContactTestsHelper))]
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
    [MemberData(nameof(ContactTestsHelper.UpdateValidTestData), MemberType = typeof(ContactTestsHelper))]
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
    [MemberData(nameof(ContactTestsHelper.UpdateNotValidTestData), MemberType = typeof(ContactTestsHelper))]
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
    [MemberData(nameof(ContactTestsHelper.DeleteValidTestData), MemberType = typeof(ContactTestsHelper))]
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
    [MemberData(nameof(ContactTestsHelper.DeleteNotValidTestData), MemberType = typeof(ContactTestsHelper))]
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

    [Fact]
    public async Task AddContactRequestValidatorTest()
    {
        ////Arrange
        //_mockRepo.Setup(repo => repo.GetById(_id)).ReturnsAsync((Contact)null);
        //_mockRepo.Setup(repo => repo.Create(_contact4).Result).Returns(_contact4);

        ////Act
        //var result1 = await _contactService.Create(_contact4req);

        ////Assert
        //Assert.NotNull(result1);
        //Assert.IsType<ContactResponse>(result1);
        //Assert.Equal(_contact4res, result1);
    }
}