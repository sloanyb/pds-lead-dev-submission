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
    
    public async Task<Person?> GetPersonByIdAsync(int personId)
    {
        return await _personRepository.GetPersonAsync(personId);
    }

    public async Task<Person> AddPersonAsync(Person newPerson)
    {
        return await _personRepository.AddPersonAsync(newPerson);
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _personRepository.GetAllAsync();
    }
}