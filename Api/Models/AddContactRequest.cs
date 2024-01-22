namespace ContactManagerCS.Models;

public class AddContactRequest
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Work { get; set; }

    public AddContactRequest(int id = 0, string name = "", string email = "", string phone = "", string? work = "")
    {
        Id = id; Name = name; Email = email; Phone = phone; Work = work;
    }

    public AddContactRequest(Contact contact)
    {
        Id = contact.Id;
        Name = contact.Name;
        Email = contact.Email;
        Phone = contact.Phone;
        Work = contact.Work;
    }
}
