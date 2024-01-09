
namespace ContactManagerCS.Models;

public class ContactResponse(int? id, string? name, string? email, string? phone, string? work)
{
    public int? Id { get; set; } = id;
    public string? Name { get; set; } = name;
    public string? Email { get; set; } = email;
    public string? Phone { get; set; } = phone;
    public string? Work { get; set; } = work;

    internal static ContactResponse Empty()
    {
        return new ContactResponse(0, "", "", "", "");
    }
}
