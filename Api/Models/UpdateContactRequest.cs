namespace ContactManagerCS.Models;

public class UpdateContactRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Company { get; set; }

    public UpdateContactRequest(int id = 0, string name = "", string email = "", string phone = "", string? company = "")
    {
        Id = id; Name = name; Email = email; Phone = phone; Company = company;
    }
}
