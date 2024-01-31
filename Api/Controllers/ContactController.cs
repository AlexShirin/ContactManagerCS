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
    public async Task<List<GetAllContactResponse>> GetAll()
    {
        return await contactService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdContactResponse>> GetById(int id)
    {
        return await contactService.GetById(id);
    }

    [HttpPost("create")]
    public async Task<ActionResult<CreateContactResponse>> Create(CreateContactRequest item)
    {
        return await contactService.Create(item);
    }

    [HttpPost("find")]
    public async Task<List<FindContactResponse>> Find(FindContactRequest find) 
    { 
        return await contactService.Find(find);
    }

    [HttpPut("update")]
    public async Task<ActionResult<UpdateContactResponse>> Update(UpdateContactRequest item)
    {
        return await contactService.Update(item);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteContactResponse>> DeleteById(int id)
    {
        return await contactService.DeleteById(id);
    }
}
