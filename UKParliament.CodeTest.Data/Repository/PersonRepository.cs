using UKParliament.CodeTest.Data.Model;

namespace UKParliament.CodeTest.Data.Repository;

public class PersonRepository(PersonManagerContext personManagerContext) : IPersonRepository
{
    public Person GetPerson(int personId)
    {
        throw new NotImplementedException();
    }
}