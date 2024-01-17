using AutoMapper;

using ContactManagerCS.Contracts;
using ContactManagerCS.Controllers;
using ContactManagerCS.Database;
using ContactManagerCS.Exceptions;
using ContactManagerCS.Models;
using ContactManagerCS.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Moq;

namespace ContactManagerCS.Tests
{
    public class ContactServiceTests
    {
        private readonly Mock<IContactRepository> _mockRepo;
        private readonly ContactService _service;
        private readonly IMapper _mapper;
        private List<Contact> _listContacts;
        private List<ContactResponse> _listContactResponses;

        public ContactServiceTests()
        {
            var contactMapperProfile = new ContactMapper();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(contactMapperProfile));
            _mapper = new Mapper(mapperConfiguration);
            _mockRepo = new Mock<IContactRepository>();
            _service = new ContactService(_mockRepo.Object, _mapper);

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
        }

        [Fact/*(Skip = "add contact mapper later")*/]
        public async Task GetAllTest()
        {
            _mockRepo.Setup(repo => repo.GetAll().Result).Returns(_listContacts);
            var result = await _service.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<ContactResponse>>(result);
            Assert.Equal(3, result.Count);
        }

        [Fact/*(Skip = "add contact mapper later")*/]
        public async Task GetByIdValidTest()
        {
            int id = 1;
            _mockRepo.Setup<Contact>(repo => repo.GetById(id).Result).Returns(_listContacts.ElementAt(id - 1));
            var result = _service.GetById(id).Result;

            Assert.NotNull(result);
            Assert.IsType<ContactResponse>(result);
            Assert.Equal(_listContactResponses.ElementAt(id - 1), result);
            //Assert.Equal(_listContactResponses.ElementAt(id-1).Id, result.Id);
            //Assert.True(_listContactResponses.ElementAt(id - 1).Equals(result));
        }

        [Fact/*(Skip = "add contact mapper later")*/]
        public async Task GetByIdNotValidTest()
        {
            int id = 4;
            _mockRepo.Setup<Contact>(repo => repo.GetById(id).Result).Returns((Contact)null);
            var ex = await Assert.ThrowsAsync<ContactException>(() => _service.GetById(id));

            Assert.NotNull(ex);
            Assert.IsType<ContactException>(ex);
            Assert.Contains("Can't GetById: contact with given Id don't exist", ex.Message);
        }

        [Fact(Skip = "add contact mapper later")]
        public async Task CreateUpdateDeleteTest()
        {
            //int id = 4;
            //Contact _contact = new Contact { Id = id, Name = "d", Email = "d@d.d", Phone = "44", Work = "D" };
            //Contact _contact2 = new Contact { Id = id, Name = "e", Email = "d@d.d", Phone = "44", Work = "D" };

            ////Create Test
            //_mockRepo
            //    .Setup(repo => repo.Create(_contact).Result)
            //    .Returns(_contact);
            ////_listContacts.Add(_contact);
            //var result1 = _controller.Create(_contact).Result.Value;

            //Assert.NotNull(result1);
            //Assert.IsType<Contact>(result1);
            //Assert.Equal(_contact, result1);

            ////Update Test
            //_mockRepo
            //    .Setup(repo => repo.Update(_contact2).Result)
            //    .Returns(_contact2);
            ////_listContacts[id].Name = _contact2.Name;
            //var result2 = _controller.Update(_contact2).Result.Value;

            //Assert.NotNull(result2);
            //Assert.IsType<Contact>(result2);
            //Assert.Equal(_contact2, result2);

            ////Delete Test
            //_mockRepo
            //    .Setup(repo => repo.DeleteById(id).Result)
            //    .Returns(_contact2);
            //var result3 = _controller.DeleteById(id).Result.Value;

            //Assert.NotNull(result3);
            //Assert.IsType<Contact>(result3);
            //Assert.Equal(_contact2, result3);
        }

        //// Arrange
        //ContactController controller = new();

        //// Act
        //List<Contact> result = controller.GetAll() as List<Contact>;

        //// Assert
        //Assert.Equal("Hello world!", result?.ViewData["Message"]);

        // Arrange
        //var mock = new Mock<ContactDbContext>();
        //mock.Setup(repo => repo.ContactItems.ToListAsync()).Returns(GetTestContacts());
        //var controller = new ContactController(mock.Object);

        //var mockContactItems = new Mock<DbSet<Contact>>();
        ////mockContactItems.Setup(ci => ci.ToListAsync().Result).Returns(GetTestContacts());
        //var mockContext = new Mock<ContactDbContext>();
        ////mockContext.Setup(c => c.ContactItems).Returns(mockContactItems.Object);
        //var mockController = new Mock<ContactController>(mockContext.Object);
        //mockController.Setup(c => c.GetAll().Result).Returns(GetTestContacts());

        ////Act
        //var result = mockController.Object.GetAll().Result;

        ////Assert
        //var viewResult = Assert.IsType<ViewResult>(result);
        //var model = Assert.IsAssignableFrom<IEnumerable<Contact>>(viewResult.Model);
        //Assert.Equal(GetTestContacts().Count, model.Count());

        //private List<Contact> GetTestContacts()
        //{
        //    var users = new List<Contact>
        //        {
        //        new Contact { Id = 1, Name = "Tom", Email = "a@a.a", Phone = "11", Work = "A" },
        //        new Contact { Id = 2, Name = "Bob", Email = "b@a.a", Phone = "22", Work = "B" },
        //        new Contact { Id = 3, Name = "Sam", Email = "c@a.a", Phone = "33", Work = "C" }
        //        };
        //    return users;
        //}
    }
}