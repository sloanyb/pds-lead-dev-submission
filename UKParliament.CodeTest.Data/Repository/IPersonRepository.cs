using UKParliament.CodeTest.Data.Model;

namespace UKParliament.CodeTest.Data.Repository;

public interface IPersonRepository
{
    Person? GetPerson(int personId);
    void AddPerson(Person newPerson);
}