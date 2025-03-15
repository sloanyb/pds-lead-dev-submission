using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Data.Repository;

namespace UKParliament.CodeTest.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    public Person? GetPersonById(int personId)
    {
        return _personRepository.GetPerson(personId);
    }

    public void AddPerson(Person newPerson)
    {
        _personRepository.AddPerson(newPerson);
    }
}