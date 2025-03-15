using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Data.Repository;
using Xunit;

namespace UKParliament.CodeTest.Data.Tests;

public class PersonRepositoryTests
{
    [Fact]
    public async Task GetById_ForExistingPerson_ReturnsPerson()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    
        using (var context = new PersonManagerContext(options))
        {
            context.People.Add(new Person { Id = 1, FirstName = "Joe", LastName = "Bloggs" });
            await context.SaveChangesAsync();
        }
    
        using (var context = new PersonManagerContext(options))
        {
            var repo = new PersonRepository(context);
            var person = await repo.GetPersonAsync(1);
        
            Assert.NotNull(person);
            Assert.Equal("Joe", person.FirstName);
            Assert.Equal("Bloggs", person.LastName);
        }
    }
    
    [Fact]
    public async Task GetById_ForNonExistingPerson_ReturnsNull()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    
        using (var context = new PersonManagerContext(options))
        {
            context.People.Add(new Person { Id = 1, FirstName = "Joe", LastName = "Bloggs" });
            await context.SaveChangesAsync();
        }
    
        using (var context = new PersonManagerContext(options))
        {
            var repo = new PersonRepository(context);
            var person = await repo.GetPersonAsync(2);

            Assert.Null(person);
        }
    }

    [Fact]
    public async Task AddingNewPerson_SavesIntoDatabase()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    
        using (var context = new PersonManagerContext(options))
        {
            var repo = new PersonRepository(context);
            var newPerson = new Person { Id = 1, FirstName = "Alice", LastName = "Wonderland" };
            await repo.AddPersonAsync(newPerson);
            await context.SaveChangesAsync();
        }
    
        using (var context = new PersonManagerContext(options))
        {
            var repo = new PersonRepository(context);
            var person = await repo.GetPersonAsync(1);
        
            Assert.NotNull(person);
            Assert.Equal("Alice", person.FirstName);
            Assert.Equal("Wonderland", person.LastName);
            Assert.True(person.Id > 0);
        }
    }

}