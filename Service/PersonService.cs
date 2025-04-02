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
}
