using System.Collections.Generic;
using ContactManagerCS.Models;

namespace ContactManagerCS.Tests.Helpers;

public class ContactTestsHelper
{
    public static IEnumerable<object[]> contactIdValid =>
        new List<object[]>
        {
            new object[] { 1 },
            new object[] { 2 },
            new object[] { 3 },
        };

    public static IEnumerable<object[]> contactIdNotValid =>
        new List<object[]>
        {
            new object[] { -1 },
            new object[] { 0 },
            new object[] { 4 },
        };

    private static List<Contact> _baseContactList => new()
        {
            new Contact { Id = 1, Name = "Tom", Email = "a@a.a", Phone = "11", Work = "A" },
            new Contact { Id = 2, Name = "Bob", Email = "b@a.a", Phone = "22", Work = "B" },
            new Contact { Id = 3, Name = "Sam", Email = "c@a.a", Phone = "33", Work = "C" },
        };
}
