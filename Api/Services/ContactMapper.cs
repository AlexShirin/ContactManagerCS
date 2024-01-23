using AutoMapper;

using ContactManagerCS.Models;

namespace ContactManagerCS.Services
{
    public class ContactMapper : Profile
    {
        public ContactMapper()
        {
            CreateMap<AddContactRequest, Contact>().ReverseMap();
            CreateMap<Contact, ContactResponse>();
        }
    }
}
