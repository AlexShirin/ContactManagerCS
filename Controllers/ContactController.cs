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
    private IContactRepository _repo;

    public ContactController(ContactDbContext context, IContactRepository repo)
    {
        _repo = repo;
    }

    /// <summary>
    /// Show all contacts
    /// </summary>
    [HttpGet]
    public async Task<List<Contact>> GetAll()
    {
        return await _repo.GetAll();
    }

    /// <summary>
    /// Shows a specific contact
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetById(int id)
    {
        return await _repo.GetById(id);
    }

    /// <summary>
    /// Create a contact
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Contact>> Create(Contact item)
    {
        return await _repo.Create(item);
    }

    /// <summary>
    /// Update a specific contact
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<Contact>> Update(Contact item)
    {
        return await _repo.Update(item);
    }

    /// <summary>
    /// Deletes a specific contact
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Contact>> DeleteById(int id)
    {
        return await _repo.DeleteById(id);
    }
}
