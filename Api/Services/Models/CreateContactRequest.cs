﻿namespace ContactManagerCS.Services.Models;

public class CreateContactRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Company { get; set; }
}
