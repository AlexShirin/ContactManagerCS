using System;
using System.Collections.Generic;
using System.Linq;

using ContactManagerCS.Models;

namespace ContactManagerCS.Tests.Helpers;

internal static class ContactHelper
{
    public static List<Contact> BaseContactList => new()
        {
            new Contact { Id = 0, Name = "Tom", Email = "a@a.a", Phone = "11", Work = "A" },
            new Contact { Id = 1, Name = "Bob", Email = "b@a.a", Phone = "22", Work = "B" },
            new Contact { Id = 2, Name = "Sam", Email = "c@a.a", Phone = "33", Work = "C" },
        };

    public static Contact ContactToAdd = new() 
        { Id = 3, Name = "Jim", Email = "d@d.d", Phone = "44", Work = "D" };
    public static Contact ContactToUpdate = new() 
        { Id = 3, Name = "Bim", Email = "d@d.d", Phone = "44", Work = "D" };
}
