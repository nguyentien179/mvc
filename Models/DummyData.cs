using System;
using mvc.Pages.Enums;

namespace mvc.Models;

public class DummyData
{
    public static List<Person> GetPeople()
    {
        return new List<Person>
        {
            new Person
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1990, 5, 15),
                PhoneNumber = "123-456-7890",
                BirthPlace = "New York",
                IsGraduated = true,
            },
            new Person
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Gender = Gender.Female,
                DateOfBirth = new DateTime(1995, 8, 22),
                PhoneNumber = "987-654-3210",
                BirthPlace = "Los Angeles",
                IsGraduated = false,
            },
            new Person
            {
                Id = 3,
                FirstName = "Alex",
                LastName = "Taylor",
                Gender = Gender.Other,
                DateOfBirth = new DateTime(2002, 1, 30),
                PhoneNumber = "555-333-7777",
                BirthPlace = "San Francisco",
                IsGraduated = false,
            },
        };
    }
}
