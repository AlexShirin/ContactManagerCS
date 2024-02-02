using System.ComponentModel.DataAnnotations;

namespace ContactManagerCS.Models;

public class Contact
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    [MaxLength(32)]
    public string Phone { get; set; }

    [MaxLength(100)]
    public string Company { get; set; }

    public Contact(int id = 0, string name = "", string email = "", string phone = "", string company = "")
    {
        Id = id; Name = name; Email = email; Phone = phone; Company = company;
    }
}
