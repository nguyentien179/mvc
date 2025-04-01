using System;
using mvc.Models;

namespace mvc.Interface;

public interface IPersonService
{
    void Create(Person person);
    void Update(Person person);
    void Delete(int id);
    List<Person> GetAll();
    void ValidatePerson(Person person);
}
