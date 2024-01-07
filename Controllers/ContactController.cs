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
    //private readonly ContactDbContext _context;
    private IContactRepository _repo;

    public ContactController(ContactDbContext context, IContactRepository repo)
    {
        //_context = context;
        _repo = repo;
    }

    /// <summary>
    /// Show all contacts
    /// </summary>
    [HttpGet]
    public async Task<List<Contact>> GetAll()
    {
        //return await _context.ContactItems.ToListAsync();
        return await _repo.GetAll();
    }

    /// <summary>
    /// Shows a specific contact
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetById(int id)
    {
        //var item = await _context.ContactItems.FindAsync(id);
        //return item is null ? NotFound() : item;
        return await _repo.GetById(id);
    }

    /// <summary>
    /// Create a contact
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Contact>> Create(Contact item)
    {
        //var exists = await _context.ContactItems.FindAsync(item.Id);
        //if (exists is not null) { return BadRequest("Contact with given Id already exists"); }

        //_context.ContactItems.Add(item);
        //await _context.SaveChangesAsync();

        //return item;
        return await _repo.Create(item);
    }

    /// <summary>
    /// Update a specific contact
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<Contact>> Update(Contact item)
    {
        //var exists = await _context.ContactItems.FindAsync(item.Id);
        //if (exists is null) { return BadRequest("Contact with given Id don't exist"); }

        //_context.ContactItems.Add(item);
        //await _context.SaveChangesAsync();

        //return item;
        return await _repo.Update(item);
    }

    /// <summary>
    /// Deletes a specific contact
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Contact>> DeleteById(int id)
    {
        //var item = await _context.ContactItems.FindAsync(id);
        //if (item is null) { return BadRequest("Contact with given Id don't exist"); }

        //_context.ContactItems.Remove(item);
        //await _context.SaveChangesAsync();

        //return item;
        return await _repo.DeleteById(id);
    }
}
