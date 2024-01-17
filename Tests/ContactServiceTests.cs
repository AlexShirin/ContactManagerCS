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

        [Fact]
        public async Task GetAllTest()
        {
            _mockRepo.Setup(repo => repo.GetAll().Result).Returns(_listContacts);
            var result = await _service.GetAll();

            Assert.NotNull(result);
            Assert.IsType<List<ContactResponse>>(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetByIdValidTest()
        {
            int id = 1;
            _mockRepo.Setup<Contact>(repo => repo.GetById(id).Result).Returns(_listContacts.ElementAt(id - 1));
            var result = await _service.GetById(id);

            Assert.NotNull(result);
            Assert.IsType<ContactResponse>(result);
            Assert.Equal(_listContactResponses.ElementAt(id - 1), result);
            //Assert.Equal(_listContactResponses.ElementAt(id-1).Id, result.Id);
            //Assert.True(_listContactResponses.ElementAt(id - 1).Equals(result));
        }

        [Fact]
        public async Task GetByIdNotValidTest()
        {
            int id = 4;
            _mockRepo.Setup<Contact>(repo => repo.GetById(id).Result).Returns((Contact)null);
            var ex = await Assert.ThrowsAsync<ContactException>(() => _service.GetById(id));

            Assert.NotNull(ex);
            Assert.IsType<ContactException>(ex);
            Assert.Contains("Can't GetById: contact with given Id don't exist", ex.Message);
        }

        [Fact]
        public async Task CreateUpdateDeleteTest()
        {
            int id = 4;
            Contact _contact4 = new Contact { Id = id, Name = "d", Email = "d@d.d", Phone = "44", Work = "D" };
            var _contact4req = new AddContactRequest { Id = id, Name = "d", Email = "d@d.d", Phone = "44", Work = "D" };
            var _contact4res = new ContactResponse { Id = id, Name = "d", Email = "d@d.d", Phone = "44", Work = "D" };
            Contact _contact4u = new Contact { Id = id, Name = "e", Email = "d@d.d", Phone = "44", Work = "D" };
            var _contact4ureq = new AddContactRequest { Id = id, Name = "e", Email = "d@d.d", Phone = "44", Work = "D" };
            var _contact4ures = new ContactResponse { Id = id, Name = "e", Email = "d@d.d", Phone = "44", Work = "D" };

            //Create Test
            _mockRepo.Setup<Contact>(repo => repo.GetById(id).Result).Returns((Contact)null);
            _mockRepo.Setup(repo => repo.Create(_contact4).Result).Returns(_contact4);
            var result1 = await _service.Create(_contact4req);

            Assert.NotNull(result1);
            Assert.IsType<ContactResponse>(result1);
            Assert.Equal(_contact4res, result1);

            //Update Test
            _mockRepo.Setup<Contact>(repo => repo.GetById(id).Result).Returns(_contact4);
            _mockRepo.Setup(repo => repo.Update(_contact4, _contact4u).Result).Returns(_contact4u);
            var result2 = await _service.Update(_contact4ureq);

            Assert.NotNull(result2);
            Assert.IsType<ContactResponse>(result2);
            Assert.Equal(_contact4ures, result2);

            //Delete Test
            _mockRepo.Setup<Contact>(repo => repo.GetById(id).Result).Returns(_contact4u);
            _mockRepo.Setup(repo => repo.Delete(_contact4u).Result).Returns(_contact4u);
            var result3 = await _service.DeleteById(id);

            Assert.NotNull(result3);
            Assert.IsType<ContactResponse>(result3);
            Assert.Equal(_contact4ures, result3);
        }
    }
}