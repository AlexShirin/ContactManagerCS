using ContactManagerCS.Contracts;
using ContactManagerCS.Models;

using Microsoft.AspNetCore.Mvc;

namespace ContactManagerCS.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ContactController : ControllerBase
{
    private readonly IContactService contactService;

    public ContactController(IContactService contactService)
    {
        this.contactService = contactService;
    }

    [HttpGet]
    public async Task<List<ContactResponse>> GetAll()
    {
        return await contactService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactResponse>> GetById(int id)
    {
        return await contactService.GetById(id);
    }

    [HttpPost]
    public async Task<ActionResult<ContactResponse>> Create(AddContactRequest item)
    {
        return await contactService.Create(item);
    }

    [HttpPut]
    public async Task<ActionResult<ContactResponse>> Update(AddContactRequest item)
    {
        return await contactService.Update(item);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ContactResponse>> DeleteById(int id)
    {
        return await contactService.DeleteById(id);
    }
}
