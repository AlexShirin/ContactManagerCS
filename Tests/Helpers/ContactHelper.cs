using ContactManagerCS.Models;
using ContactManagerCS.Services.Models;

namespace ContactManagerCS.Tests.Helpers;

internal static class ContactHelper
{
    public static List<Contact> BaseContactList => new()
        {
            new Contact { Id = 1, Name = "Tom", Email = "a@a.a", Phone = "11", Company = "A" },
            new Contact { Id = 2, Name = "Bob", Email = "b@a.a", Phone = "22", Company = "B" },
            new Contact { Id = 3, Name = "Sam", Email = "c@a.a", Phone = "33", Company = "C" },
        };

    public static CreateContactRequest ContactToCreate = new() 
        {Name = "Jim", Email = "d@d.d", Phone = "44", Company = "D" };
    public static UpdateContactRequest ContactToUpdate = new() 
        { Id = 4, Name = "Bim", Email = "d@d.d", Phone = "44", Company = "D" };
}
