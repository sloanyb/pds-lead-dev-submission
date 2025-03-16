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
            await context.Database.EnsureCreatedAsync();

            context.People.Add(new Person
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Bloggs",
                DepartmentId = 3,
                DateOfBirth = new DateOnly(1980, 02, 03)
            });

            await context.SaveChangesAsync();
        }

        using (var context = new PersonManagerContext(options))
        {
            await context.Database.EnsureCreatedAsync();

            var repo = new PersonRepository(context);
            var person = await repo.GetPersonAsync(1);

            Assert.NotNull(person);
            Assert.Equal("Joe", person.FirstName);
            Assert.Equal("Bloggs", person.LastName);
            Assert.Equal(3, person.DepartmentId);
            Assert.Equal("Finance", person.Department.Name);
            Assert.Equal(new DateOnly(1980, 02, 03), person.DateOfBirth);
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
            await context.Database.EnsureCreatedAsync();

            context.People.Add(new Person
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Bloggs",
                DepartmentId = 2,
                DateOfBirth = new DateOnly(1990, 12, 25)
            });
            await context.SaveChangesAsync();
        }

        using (var context = new PersonManagerContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            
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
            await context.Database.EnsureCreatedAsync();
            
            context.People.Add(new Person
            {
                Id = 1, FirstName = "Kathryn", LastName = "Janeway", DepartmentId = 1,
                DateOfBirth = new DateOnly(2324, 05, 04)
            });
            context.People.Add(new Person
            {
                Id = 2, FirstName = "Tom", LastName = "Paris", DepartmentId = 2,
                DateOfBirth = new DateOnly(2329, 07, 21)
            });
            context.People.Add(new Person
            {
                Id = 3, FirstName = "Harry", LastName = "Kim", DepartmentId = 3,
                DateOfBirth = new DateOnly(2330, 11, 15)
            });
            context.People.Add(new Person
            {
                Id = 4, FirstName = "B'Elanna", LastName = "Torres", DepartmentId = 4,
                DateOfBirth = new DateOnly(2328, 09, 05)
            });

            await context.SaveChangesAsync();
        }

        using (var context = new PersonManagerContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            
            var repo = new PersonRepository(context);
            var people = (await repo.GetAllAsync()).ToList();

            Assert.True(people.Count() == 4);

            Assert.Equal("Kathryn", people[0].FirstName);
            Assert.Equal("Janeway", people[0].LastName);
            Assert.Equal(new DateOnly(2324, 05, 04), people[0].DateOfBirth);
            Assert.NotNull(people[0].Department);
            Assert.Equal("Sales", people[0].Department.Name);

            Assert.Equal("Tom", people[1].FirstName);
            Assert.Equal("Paris", people[1].LastName);
            Assert.Equal(new DateOnly(2329, 07, 21), people[1].DateOfBirth);
            Assert.NotNull(people[1].Department);
            Assert.Equal("Marketing", people[1].Department.Name);

            Assert.Equal("Harry", people[2].FirstName);
            Assert.Equal("Kim", people[2].LastName);
            Assert.Equal(new DateOnly(2330, 11, 15), people[2].DateOfBirth);
            Assert.NotNull(people[2].Department);
            Assert.Equal("Finance", people[2].Department.Name);

            Assert.Equal("B'Elanna", people[3].FirstName);
            Assert.Equal("Torres", people[3].LastName);
            Assert.Equal(new DateOnly(2328, 09, 05), people[3].DateOfBirth);
            Assert.NotNull(people[3].Department);
            Assert.Equal("HR", people[3].Department.Name);
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
            await context.Database.EnsureCreatedAsync();
            
            var repo = new PersonRepository(context);

            var newPerson = new Person
            {
                Id = 1,
                FirstName = "Alice",
                LastName = "Wonderland",
                DepartmentId = 1,
                DateOfBirth = new DateOnly(1920, 06, 12)
            };
            await repo.AddPersonAsync(newPerson);
            await context.SaveChangesAsync();
        }

        using (var context = new PersonManagerContext(options))
        {
            await context.Database.EnsureCreatedAsync();
            
            var repo = new PersonRepository(context);
            var person = await repo.GetPersonAsync(1);

            Assert.NotNull(person);
            Assert.Equal("Alice", person.FirstName);
            Assert.Equal("Wonderland", person.LastName);
            Assert.Equal(1, person.DepartmentId);
            Assert.Equal("Sales", person.Department.Name);
            Assert.True(person.Id > 0);
        }
    }
}