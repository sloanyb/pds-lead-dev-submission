using UKParliament.CodeTest.Data.Repository;

namespace UKParliament.CodeTest.Services;

public class PersonService(IPersonRepository personManagerRepository) : IPersonService
{
    private readonly IPersonRepository _personManagerRepository = personManagerRepository;
}