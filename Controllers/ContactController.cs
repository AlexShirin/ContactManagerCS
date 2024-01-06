using ContactManagerCS.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ContactController : ControllerBase
{
    private readonly ContactDbContext _context;

    public ContactController(ContactDbContext context) => _context = context;

    /// <summary>
    /// Show all contacts
    /// </summary>
    [HttpGet]
    public async Task<List<Contact>> GetAll() => await _context.ContactItems.ToListAsync();

    /// <summary>
    /// Shows a specific contact
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetById(int id)
    {
        var item = await _context.ContactItems.FindAsync(id);

        return item is null ? NotFound() : item;
    }

    /// <summary>
    /// Create a contact
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Contact>> Create(Contact item)
    {
        var exists = await _context.ContactItems.FindAsync(item.Id);
        if (exists is not null) { return BadRequest("Contact with given Id already exists"); }

        _context.ContactItems.Add(item);
        await _context.SaveChangesAsync();

        return item;
    }

    /// <summary>
    /// Update a specific contact
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<Contact>> Update(Contact item)
    {
        var exists = await _context.ContactItems.FindAsync(item.Id);
        if (exists is null) { return BadRequest("Contact with given Id don't exist"); }

        _context.ContactItems.Add(item);
        await _context.SaveChangesAsync();

        return item;
    }

    /// <summary>
    /// Deletes a specific contact
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Contact>> DeleteById(int id)
    {
        var item = await _context.ContactItems.FindAsync(id);
        if (item is null) { return BadRequest("Contact with given Id don't exist"); }

        _context.ContactItems.Remove(item);
        await _context.SaveChangesAsync();

        return item;
    }
}
