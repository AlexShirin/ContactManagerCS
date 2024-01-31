namespace ContactManagerCS.Models;

public class DeleteContactResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? Company { get; set; }

    public DeleteContactResponse(int id, string name, string email, string phone, string? company)
    {
        Id = id; Name = name; Email = email; Phone = phone; Company = company;
    }
}
