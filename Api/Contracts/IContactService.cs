using ContactManagerCS.Models;

namespace ContactManagerCS.Contracts;

public interface IContactService
{
    Task<ContactResponse> Create(AddContactRequest item);
    Task<ContactResponse> DeleteById(int id);
    Task<List<ContactResponse>> GetAll();
    Task<ContactResponse> GetById(int id);
    Task<ContactResponse> Update(AddContactRequest item);
}
