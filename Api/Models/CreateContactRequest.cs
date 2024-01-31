namespace ContactManagerCS.Models;

public class CreateContactRequest
{
    private int Id { get; set; } = -1;
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Company { get; set; }

    public CreateContactRequest(string name = "", string email = "", string phone = "", string? company = "")
    {
        Name = name; Email = email; Phone = phone; Company = company;
    }
}
