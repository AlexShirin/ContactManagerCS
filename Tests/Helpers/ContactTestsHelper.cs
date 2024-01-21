using System.Collections.Generic;
using System.Linq;

using ContactManagerCS.Models;

namespace ContactManagerCS.Tests.Helpers;

public class ContactTestsHelper
{
    private static object[] _validContactIds = { 1, 2, 3 };
    private static object[] _notvalidContactIds = { -1, 0, 4 };

    private static List<Contact> _baseContactList => new()
        {
            new Contact { Id = 1, Name = "Tom", Email = "a@a.a", Phone = "11", Work = "A" },
            new Contact { Id = 2, Name = "Bob", Email = "b@a.a", Phone = "22", Work = "B" },
            new Contact { Id = 3, Name = "Sam", Email = "c@a.a", Phone = "33", Work = "C" },
        };

    //public static IEnumerable<object[]> ContactIdValid =>
    //    new List<object[]>
    //    {
    //        new object[] { 1 },
    //        new object[] { 2 },
    //        new object[] { 3 },
    //    };

    //public static IEnumerable<object[]> ContactIdNotValid =>
    //    new List<object[]>
    //    {
    //        new object[] { -1 },
    //        new object[] { 0 },
    //        new object[] { 4 },
    //    };

    public static IEnumerable<object[]> GetValidContactIds()
    {
        foreach (var contactId in _validContactIds)
        {
            object[] tmp = [contactId];
            yield return tmp;
        }
    }

    public static IEnumerable<object[]> GetNotValidContactIds()
    {
        foreach (var contactId in _notvalidContactIds)
        {
            object[] tmp = [contactId];
            yield return tmp;
        }
    }

    public static IEnumerable<object[]> GetListOfContacts()
    {
        object[] tmp = [_baseContactList];
        yield return tmp;
    }
}
