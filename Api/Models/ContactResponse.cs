
namespace ContactManagerCS.Models
{
    public class ContactResponse
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Work { get; set; }

        public ContactResponse(int id = 0, string name = "", string email = "", string phone = "", string? work = "")
        {
            Id = id; Name = name; Email = email; Phone = phone; Work = work;
        }

        public static ContactResponse Empty()
        {
            return new ContactResponse(0, "", "", "", "");
        }
    }
}
