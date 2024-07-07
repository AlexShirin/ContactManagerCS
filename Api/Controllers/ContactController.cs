using ContactManagerCS.Services.Models;
using ContactManagerCS.Services;
using Microsoft.AspNetCore.Mvc;
using ContactManagerCS.Common.ApiKeyAuthentication;
using ContactManagerCS.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactManagerCS.Controllers;

[Authorize]
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
    public async Task<List<GetAllContactResponse>> GetAll()
    {
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

    //[HttpPost("login")]
    //public IActionResult Login([FromBody] LoginModel login)
    //{
    //    if (IsValidUser(login))
    //    {
    //        var token = GenerateJwtToken();
    //        return Ok(new { token });
    //    }

    //    return Unauthorized();
    //}

    //public class LoginModel
    //{
    //    public required string Username { get; set; }
    //    public required string Password { get; set; }
    //}

    //private bool IsValidUser(LoginModel login)
    //{
    //    // Implement your user validation logic here
    //    return login.Username == "test" && login.Password == "password";
    //}

    //private string GenerateJwtToken()
    //{
    //    var tokenHandler = new JwtSecurityTokenHandler();
    //    var key = Encoding.UTF8.GetBytes("my_secret_key_here12345678901234567890");
    //    var tokenDescriptor = new SecurityTokenDescriptor
    //    {
    //        Subject = new ClaimsIdentity(new[]
    //        {
    //            new Claim(ClaimTypes.Name, "test")
    //        }),
    //        Expires = DateTime.UtcNow.AddHours(1),
    //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    //    };
    //    var token = tokenHandler.CreateToken(tokenDescriptor);
    //    return tokenHandler.WriteToken(token);
    //}
}
