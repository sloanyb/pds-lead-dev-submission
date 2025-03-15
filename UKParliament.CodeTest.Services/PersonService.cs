using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public class PersonService(PersonManagerContext personManagerContext) : IPersonService
{
    private readonly PersonManagerContext _personManagerContext = personManagerContext;
}