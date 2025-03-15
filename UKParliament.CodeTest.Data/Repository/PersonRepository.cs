using UKParliament.CodeTest.Data.Model;

namespace UKParliament.CodeTest.Data.Repository;

public class PersonRepository(PersonManagerContext personManagerContext) : IPersonRepository
{
    public Person? GetPerson(int personId)
    {
        return personManagerContext.People.SingleOrDefault(x => x.Id == personId);
    }
}