using ContactManagerCS.Contracts;
using ContactManagerCS.Database;
using ContactManagerCS.Models;
using ContactManagerCS.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    /// <summary>
    /// Show all contacts
    /// </summary>
    [HttpGet]
    public async Task<List<ContactResponse>> GetAll()
    {
        return await contactService.GetAll();
    }

    /// <summary>
    /// Shows a specific contact
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ContactResponse>> GetById(int id)
    {
        return await contactService.GetById(id);
    }

    /// <summary>
    /// Create a contact
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ContactResponse>> Create(AddContactRequest item)
    {
        return await contactService.Create(item);
    }

    /// <summary>
    /// Update a specific contact
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<ContactResponse>> Update(AddContactRequest item)
    {
        return await contactService.Update(item);
    }

    /// <summary>
    /// Deletes a specific contact
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ContactResponse>> DeleteById(int id)
    {
        return await contactService.DeleteById(id);
    }
}
