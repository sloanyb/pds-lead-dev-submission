using FakeItEasy;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Data.Repository;
using Xunit;

namespace UKParliament.CodeTest.Services.Tests;

public class PersonServiceTests
{
    [Fact]
    public void PersonService_WhenPersonExists_ReturnsPerson()
    {
        var fakePerson = new Person()
        {
            Id = 1,
            FirstName = "Joe",
            LastName = "Bloggs"
        };
        
        var fakePersonRepo = A.Fake<IPersonRepository>();
        
        A.CallTo(() => fakePersonRepo.GetPerson(1)).Returns(fakePerson);
        
        var service = new PersonService(fakePersonRepo);

        var person = service.GetPersonById(1);
        
        Assert.NotNull(fakePerson);
        Assert.Equal("Bloggs", person.LastName);
        Assert.Equal("Joe", person.FirstName);
        Assert.Equal(1, person.Id);
    }
    
    [Fact]
    public void PersonService_AddPerson_CallsRepositoryAddPerson()
    {
        var fakePersonRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(fakePersonRepo);
        var newPerson = new Person
        {
            FirstName = "Alice",
            LastName = "Smith"
        };

        var savedPerson = service.AddPerson(newPerson);
        A.CallTo(() => fakePersonRepo.AddPerson(newPerson)).MustHaveHappenedOnceExactly();
    }
}