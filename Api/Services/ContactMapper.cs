using AutoMapper;

using ContactManagerCS.Models;

namespace ContactManagerCS.Services;

public class ContactMapper : Profile
{
    public ContactMapper()
    {
        CreateMap<CreateContactRequest, Contact>().ReverseMap();
        CreateMap<FindContactRequest, Contact>().ReverseMap();
        CreateMap<UpdateContactRequest, Contact>().ReverseMap();

        CreateMap<Contact, GetAllContactResponse>();
        CreateMap<Contact, GetByIdContactResponse>();
        CreateMap<Contact, CreateContactResponse>();
        CreateMap<Contact, FindContactResponse>();
        CreateMap<Contact, DeleteContactResponse>();
        CreateMap<Contact, UpdateContactResponse>();
    }
}
