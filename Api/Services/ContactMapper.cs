using AutoMapper;

using ContactManagerCS.Models;
using ContactManagerCS.Services.Models;

namespace ContactManagerCS.Services;

public class ContactMapper : Profile
{
    public ContactMapper()
    {
        CreateMap<CreateContactRequest, Contact>();
        CreateMap<FindContactRequest, Contact>();
        CreateMap<UpdateContactRequest, Contact>();

        CreateMap<Contact, GetAllContactResponse>();
        CreateMap<Contact, GetByIdContactResponse>();
        CreateMap<Contact, CreateContactResponse>();
        CreateMap<Contact, FindContactResponse>();
        CreateMap<Contact, DeleteContactResponse>();
        CreateMap<Contact, UpdateContactResponse>();
    }
}
