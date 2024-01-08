using ContactManagerCS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Contracts;

public interface IContactRepository
{
    public Task<List<ContactResponse>> GetAll();
    public Task<ContactResponse> GetById(int id);
    public Task<ContactResponse> Create(AddContactRequest item);
    public Task<ContactResponse> Update(AddContactRequest item);
    public Task<ContactResponse> DeleteById(int id);
}
