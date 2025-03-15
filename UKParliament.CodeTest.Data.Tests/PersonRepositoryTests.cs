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
    public async Task GetAll_ReturnsAllPeople()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    
        using (var context = new PersonManagerContext(options))
        {
            context.People.Add(new Person { Id = 1, FirstName = "Kathryn", LastName = "Janeway" });
            context.People.Add(new Person { Id = 2, FirstName = "Tom", LastName = "Paris" });
            context.People.Add(new Person { Id = 3, FirstName = "Harry", LastName = "Kim" });
            context.People.Add(new Person { Id = 4, FirstName = "B'Elanna", LastName = "Torres" });
            
            await context.SaveChangesAsync();
        }
    
        using (var context = new PersonManagerContext(options))
        {
            var repo = new PersonRepository(context);
            var people = (await repo.GetAllAsync()).ToList();

            Assert.True(people.Count() == 4);
            
            // Assert on individual person's names based on the ordering
            Assert.Equal("Kathryn", people[0].FirstName);
            Assert.Equal("Janeway", people[0].LastName);

            Assert.Equal("Tom", people[1].FirstName);
            Assert.Equal("Paris", people[1].LastName);

            Assert.Equal("Harry", people[2].FirstName);
            Assert.Equal("Kim", people[2].LastName);

            Assert.Equal("B'Elanna", people[3].FirstName);
            Assert.Equal("Torres", people[3].LastName);

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