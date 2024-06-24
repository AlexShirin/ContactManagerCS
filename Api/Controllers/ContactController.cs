using ContactManagerCS.Services.Models;
using ContactManagerCS.Services;
using Microsoft.AspNetCore.Mvc;
using ContactManagerCS.Common.ApiKeyAuthentication;
using ContactManagerCS.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace ContactManagerCS.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly IApiKeyValidation _apiKeyValidation;

    public ContactController(
        IContactService contactService, 
        IApiKeyValidation apiKeyValidation)
    {
        _contactService = contactService;
        _apiKeyValidation = apiKeyValidation;
    }

    [HttpGet]
    //[ApiKey]
    [Authorize]
    public async Task<List<GetAllContactResponse>> GetAll(/*string apiKey*/)
    {
        //if (string.IsNullOrWhiteSpace(apiKey)) throw new ContactException("ApiKey is missing");

        //bool isValid = _apiKeyValidation.IsValidApiKey(apiKey);
        //if (!isValid) throw new ContactException("ApiKey is invalid");

        return await _contactService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetByIdContactResponse>> GetById(int id)
    {
        return await _contactService.GetById(id);
    }

    [HttpPost("create")]
    public async Task<ActionResult<CreateContactResponse>> Create(CreateContactRequest request)
    {
        return await _contactService.Create(request);
    }

    [HttpPost("find")]
    public async Task<List<FindContactResponse>> Find(FindContactRequest request)
    {
        return await _contactService.Find(request);
    }

    [HttpPut("update")]
    public async Task<ActionResult<UpdateContactResponse>> Update(UpdateContactRequest request)
    {
        return await _contactService.Update(request);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<DeleteContactResponse>> DeleteById(int id)
    {
        return await _contactService.DeleteById(id);
    }
}
