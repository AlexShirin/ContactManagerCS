using System;
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

    private static Contact _contactToAdd = new() 
        { Id = 4, Name = "Jim", Email = "d@d.d", Phone = "44", Work = "D" };
    private static Contact _contactToUpdate = new() 
        { Id = 4, Name = "Bim", Email = "d@d.d", Phone = "44", Work = "D" };
    
    private static List<ContactResponse> _baseContactResponseList => new()
        {
            new ContactResponse(_baseContactList.ElementAt(0)),
            new ContactResponse(_baseContactList.ElementAt(1)),
            new ContactResponse(_baseContactList.ElementAt(2)),
        };
    private static List<AddContactRequest> _baseAddContactRequestList => new()
        {
            new AddContactRequest(_baseContactList.ElementAt(0)),
            new AddContactRequest(_baseContactList.ElementAt(1)),
            new AddContactRequest(_baseContactList.ElementAt(2)),
        };

    public static IEnumerable<object[]> GetAllTestData()
    {
        object[] tmp = [_baseContactList];
        yield return tmp;
    }

    public static IEnumerable<object[]> GetByIdValidTestData()
    {
        foreach (var contactId in _validContactIds)
        {
            //object[] tmp = [contactId];
            yield return [contactId, _baseContactList];
        }
    }

    public static IEnumerable<object[]> GetByIdNotValidTestData()
    {
        var allData = new List<object[]> { new object[] { _notvalidContactIds.GetValue(0) } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> CreateValidTestData()
    {
        var allData = new List<object[]> { new object[] { _contactToAdd } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> CreateNotValidTestData()
    {
        var allData = new List<object[]> { new object[] { _contactToAdd } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> UpdateValidTestData()
    {
        var allData = new List<object[]> { new object[] { _contactToAdd, _contactToUpdate } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> UpdateNotValidTestData()
    {
        var allData = new List<object[]> { new object[] { _contactToAdd, _contactToUpdate } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> DeleteValidTestData()
    {
        var allData = new List<object[]> { new object[] { _contactToUpdate } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> DeleteNotValidTestData()
    {
        var allData = new List<object[]> { new object[] { _contactToUpdate } };
        return allData.Take(1);
    }
}
