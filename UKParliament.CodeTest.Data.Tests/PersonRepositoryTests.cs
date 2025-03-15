using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Data.Repository;
using Xunit;

namespace UKParliament.CodeTest.Data.Tests;

public class PersonRepositoryTests
{
    [Fact]
    public void GetById_ForExistingPerson_ReturnsPerson()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    
        using (var context = new PersonManagerContext(options))
        {
            context.People.Add(new Person { Id = 1, FirstName = "Joe", LastName = "Bloggs" });
            context.SaveChanges();
        }
    
        using (var context = new PersonManagerContext(options))
        {
            var repo = new PersonRepository(context);
            var person = repo.GetPerson(1);
        
            Assert.NotNull(person);
            Assert.Equal("Joe", person.FirstName);
            Assert.Equal("Bloggs", person.LastName);
        }
    }
    
    [Fact]
    public void GetById_ForNonExistingPerson_ReturnsNull()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    
        using (var context = new PersonManagerContext(options))
        {
            context.People.Add(new Person { Id = 1, FirstName = "Joe", LastName = "Bloggs" });
            context.SaveChanges();
        }
    
        using (var context = new PersonManagerContext(options))
        {
            var repo = new PersonRepository(context);
            var person = repo.GetPerson(2);

            Assert.Null(person);
        }
    }

    [Fact]
    public void AddingNewPerson_SavesIntoDatabase()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    
        using (var context = new PersonManagerContext(options))
        {
            var repo = new PersonRepository(context);
            var newPerson = new Person { Id = 1, FirstName = "Alice", LastName = "Wonderland" };
            repo.AddPerson(newPerson);
            context.SaveChanges();
        }
    
        using (var context = new PersonManagerContext(options))
        {
            var repo = new PersonRepository(context);
            var person = repo.GetPerson(1);
        
            Assert.NotNull(person);
            Assert.Equal("Alice", person.FirstName);
            Assert.Equal("Wonderland", person.LastName);
            Assert.True(person.Id > 0);
        }
    }

}