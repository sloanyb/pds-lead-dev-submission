using FakeItEasy;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Data.Repository;
using Xunit;

namespace UKParliament.CodeTest.Services.Tests;

public class PersonServiceTests
{
    [Fact]
    public async Task PersonService_WhenPersonExists_ReturnsPerson()
    {
        var fakePerson = new Person()
        {
            Id = 1,
            FirstName = "Joe",
            LastName = "Bloggs"
        };
        
        var fakePersonRepo = A.Fake<IPersonRepository>();
        
        A.CallTo(() => fakePersonRepo.GetPersonAsync(1))
            .Returns(Task.FromResult(fakePerson));
        
        var service = new PersonService(fakePersonRepo);

        var person = await service.GetPersonByIdAsync(1);
        
        Assert.NotNull(fakePerson);
        Assert.Equal("Bloggs", person.LastName);
        Assert.Equal("Joe", person.FirstName);
        Assert.Equal(1, person.Id);
    }
    
    [Fact]
    public async Task PersonService_AddPerson_CallsRepositoryAddPerson()
    {
        var fakePersonRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(fakePersonRepo);
        var newPerson = new Person
        {
            FirstName = "Alice",
            LastName = "Smith"
        };

        var savedPerson = await service.AddPersonAsync(newPerson);
        A.CallTo(() => fakePersonRepo.AddPersonAsync(newPerson)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task PersonService_GetAllAsync_ReturnsAllPeople()
    {
        var fakePeople = new List<Person>
        {
            new() { Id = 1, FirstName = "Alice", LastName = "Smith" },
            new() { Id = 2, FirstName = "Bob", LastName = "Jones" },
            new() { Id = 3, FirstName = "Charlie", LastName = "Brown" }
        };

        var fakePersonRepo = A.Fake<IPersonRepository>();
        A.CallTo(() => fakePersonRepo.GetAllAsync())
            .Returns(Task.FromResult<IEnumerable<Person>>(fakePeople));

        var service = new PersonService(fakePersonRepo);

        var result = await service.GetAllAsync();

        Assert.Equal(3, result.Count());

        Assert.Contains(result, p => p.Id == 1 && p.FirstName == "Alice" && p.LastName == "Smith");
        Assert.Contains(result, p => p.Id == 2 && p.FirstName == "Bob" && p.LastName == "Jones");
        Assert.Contains(result, p => p.Id == 3 && p.FirstName == "Charlie" && p.LastName == "Brown");
    }
    
    [Fact]
    public async Task PersonService_UpdatePerson_ReturnsUpdatedPerson()
    {
        var originalPerson = new Person { Id = 1, FirstName = "Joe", LastName = "Bloggs" };
        var updatedPerson = new Person { Id = 1, FirstName = "Joseph", LastName = "Bloggs" };

        var fakePersonRepo = A.Fake<IPersonRepository>();
        A.CallTo(() => fakePersonRepo.UpdatePersonAsync(A<Person>.That.Matches(
                p => p.Id == originalPerson.Id &&
                     p.FirstName == originalPerson.FirstName &&
                     p.LastName == originalPerson.LastName)))
            .Returns(Task.FromResult(updatedPerson));

        var service = new PersonService(fakePersonRepo);

        var result = await service.UpdatePersonAsync(originalPerson);

        Assert.Equal("Joseph", result.FirstName);
        
        A.CallTo(() => fakePersonRepo.UpdatePersonAsync(A<Person>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
}