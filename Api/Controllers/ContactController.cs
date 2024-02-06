using ContactManagerCS.Services.Models;
using ContactManagerCS.Services;
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
    public async Task<ActionResult<CreateContactResponse>> Create(CreateContactRequest request)
    {
        return await contactService.Create(request);
    }

    [HttpPost("find")]
    public async Task<List<FindContactResponse>> Find(FindContactRequest request)
    {
        return await contactService.Find(request);
    }

    [HttpPut("update")]
    public async Task<ActionResult<UpdateContactResponse>> Update(UpdateContactRequest request)
    {
        return await contactService.Update(request);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteContactResponse>> DeleteById(int id)
    {
        return await contactService.DeleteById(id);
    }
}
