using ContactManagerCS.Services.Models;

namespace ContactManagerCS.Services;

public interface IContactService
{
    Task<List<GetAllContactResponse>> GetAll();
    Task<GetByIdContactResponse> GetById(int id);
    Task<CreateContactResponse> Create(CreateContactRequest create);
    Task<DeleteContactResponse> DeleteById(int id);
    Task<List<FindContactResponse>> Find(FindContactRequest find);
    Task<UpdateContactResponse> Update(UpdateContactRequest update);
}
