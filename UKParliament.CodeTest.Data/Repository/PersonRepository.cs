using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Model;

namespace UKParliament.CodeTest.Data.Repository;

public class PersonRepository(PersonManagerContext personManagerContext) : IPersonRepository
{
    public async Task<Person?> GetPersonAsync(int personId)
    {
        return await personManagerContext.People.SingleOrDefaultAsync(x => x.Id == personId);
    }

    public async Task<Person> AddPersonAsync(Person newPerson)
    {
        personManagerContext.People.Add(newPerson);
        await personManagerContext.SaveChangesAsync();
        
        return await GetPersonAsync(newPerson.Id)!;
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await personManagerContext.People.ToListAsync();
    }
    
    public async Task<Person> UpdatePersonAsync(Person person)
    {
        personManagerContext.People.Update(person);
        await personManagerContext.SaveChangesAsync();
        
        return person;
    }
}