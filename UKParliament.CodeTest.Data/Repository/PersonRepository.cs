using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Model;

namespace UKParliament.CodeTest.Data.Repository;

public class PersonRepository : IPersonRepository
{
    private readonly PersonManagerContext _personManagerContext;

    public PersonRepository(PersonManagerContext personManagerContext)
    {
        _personManagerContext = personManagerContext;
        _personManagerContext.Database.EnsureCreated();
    }

    public async Task<Person?> GetPersonAsync(int personId)
    {
        return await _personManagerContext.People.Include(p => p.Department).SingleOrDefaultAsync(x => x.Id == personId);
    }

    public async Task<Person> AddPersonAsync(Person newPerson)
    {
        _personManagerContext.People.Add(newPerson);
        await _personManagerContext.SaveChangesAsync();
        
        return await GetPersonAsync(newPerson.Id)!;
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _personManagerContext.People.Include(p => p.Department).ToListAsync();
    }
    
    public async Task<Person> UpdatePersonAsync(Person person)
    {
        _personManagerContext.People.Update(person);
        await _personManagerContext.SaveChangesAsync();
        
        return (await GetPersonAsync(person.Id))!;
    }
}