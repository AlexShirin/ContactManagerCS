using System.ComponentModel.DataAnnotations;

namespace ContactManagerCS.DAL.Entities;

public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Company { get; set; }
}
