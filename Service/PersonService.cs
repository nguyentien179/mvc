using System;
using mvc.Interface;
using mvc.Models;

namespace mvc.Service;

public class PersonService : IPersonService
{
    private List<Person> _people;

    public PersonService()
    {
        _people = DummyData.GetPeople();
    }

    public void Create(Person person)
    {
        person.Id = _people.Count > 0 ? _people.Max(p => p.Id) + 1 : 1;
        _people.Add(person);
    }

    public void Update(Person person)
    {
        var existingPerson = _people.FirstOrDefault(p => p.Id == person.Id);
        if (existingPerson == null)
        {
            throw new Exception("Person not found.");
        }
        existingPerson.FirstName = person.FirstName;
        existingPerson.LastName = person.LastName;
        existingPerson.Gender = person.Gender;
        existingPerson.DateOfBirth = person.DateOfBirth;
        existingPerson.BirthPlace = person.BirthPlace;
        existingPerson.IsGraduated = person.IsGraduated;
    }

    public void Delete(int id)
    {
        var person = _people.FirstOrDefault(p => p.Id == id);
        if (person == null)
        {
            throw new Exception("Person not found.");
        }
        _people.Remove(person);
    }

    public List<Person> GetAll()
    {
        return _people;
    }

    public void ValidatePerson(Person person)
    {
        if (_people.Any(p => p.Id == person.Id))
        {
            throw new Exception("Person with this ID already exists.");
        }
        if (string.IsNullOrEmpty(person.FirstName) || string.IsNullOrEmpty(person.LastName))
        {
            throw new Exception("First name and last name are required.");
        }
        if (person.DateOfBirth > DateTime.Now)
        {
            throw new Exception("Date of birth cannot be in the future.");
        }
        if (person.DateOfBirth.Year < 1900 || person.DateOfBirth.Year > DateTime.Now.Year)
        {
            throw new Exception("Date of birth must be between 1900 and the current year.");
        }
        if (string.IsNullOrEmpty(person.PhoneNumber) || person.PhoneNumber.Length < 10)
        {
            throw new Exception("Phone number is required and must be at least 10 digits long.");
        }
        if (string.IsNullOrEmpty(person.BirthPlace))
        {
            throw new Exception("Birth place is required.");
        }
    }
}
