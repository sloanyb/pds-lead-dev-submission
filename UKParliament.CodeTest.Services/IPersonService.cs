using UKParliament.CodeTest.Data.Model;

namespace UKParliament.CodeTest.Services;

public interface IPersonService
{
    Person? GetPersonById(int personId);
    void AddPerson(Person newPerson);
}