﻿using ContactManagerCS.Contracts;
using ContactManagerCS.Database;
using ContactManagerCS.Models;
using ContactManagerCS.Repositories;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerCS.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ContactController(IContactRepository repo) : ControllerBase
{
    /// <summary>
    /// Show all contacts
    /// </summary>
    [HttpGet]
    public async Task<List<Contact>> GetAll()
    {
        return await repo.GetAll();
    }

    /// <summary>
    /// Shows a specific contact
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetById(int id)
    {
        return await repo.GetById(id);
    }

    /// <summary>
    /// Create a contact
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Contact>> Create(Contact item)
    {
        return await repo.Create(item);
    }

    /// <summary>
    /// Update a specific contact
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<Contact>> Update(Contact item)
    {
        return await repo.Update(item);
    }

    /// <summary>
    /// Deletes a specific contact
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<Contact>> DeleteById(int id)
    {
        return await repo.DeleteById(id);
    }
}
