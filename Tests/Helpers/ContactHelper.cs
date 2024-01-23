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
    public static List<ContactResponse> BaseContactResponseList => new()
        {
            new ContactResponse { Id = 0, Name = "Tom", Email = "a@a.a", Phone = "11", Work = "A" },
            new ContactResponse { Id = 1, Name = "Bob", Email = "b@a.a", Phone = "22", Work = "B" },
            new ContactResponse { Id = 2, Name = "Sam", Email = "c@a.a", Phone = "33", Work = "C" },
        };
    public static List<AddContactRequest> BaseAddContactRequestList => new()
        {
            new AddContactRequest { Id = 0, Name = "Tom", Email = "a@a.a", Phone = "11", Work = "A" },
            new AddContactRequest { Id = 1, Name = "Bob", Email = "b@a.a", Phone = "22", Work = "B" },
            new AddContactRequest { Id = 2, Name = "Sam", Email = "c@a.a", Phone = "33", Work = "C" },
        };

    public static object[] ValidContactIds = { 0, 1, 2 };
    public static object[] NotValidContactIds = { -1, 3 };

    public static Contact ContactToAdd = new() 
        { Id = 3, Name = "Jim", Email = "d@d.d", Phone = "44", Work = "D" };
    public static Contact ContactToUpdate = new() 
        { Id = 3, Name = "Bim", Email = "d@d.d", Phone = "44", Work = "D" };


    //public static IEnumerable<object[]> GetAllTestData()
    //{
    //    object[] tmp = [BaseContactList];
    //    yield return tmp;
    //}

    //public static IEnumerable<object[]> GetByIdValidTestData()
    //{
    //    foreach (var contactId in ValidContactIds)
    //    {
    //        //object[] tmp = [contactId];
    //        yield return [contactId, BaseContactList];
    //    }
    //}

    //public static IEnumerable<object[]> GetByIdNotValidTestData()
    //{
    //    var allData = new List<object[]> { new object[] { NotValidContactIds.GetValue(0) } };
    //    return allData.Take(1);
    //}

    public static IEnumerable<object[]> CreateValidTestData()
    {
        var allData = new List<object[]> { new object[] { ContactToAdd } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> CreateNotValidTestData()
    {
        var allData = new List<object[]> { new object[] { ContactToAdd } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> UpdateValidTestData()
    {
        var allData = new List<object[]> { new object[] { ContactToAdd, ContactToUpdate } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> UpdateNotValidTestData()
    {
        var allData = new List<object[]> { new object[] { ContactToAdd, ContactToUpdate } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> DeleteValidTestData()
    {
        var allData = new List<object[]> { new object[] { ContactToUpdate } };
        return allData.Take(1);
    }

    public static IEnumerable<object[]> DeleteNotValidTestData()
    {
        var allData = new List<object[]> { new object[] { ContactToUpdate } };
        return allData.Take(1);
    }
}
