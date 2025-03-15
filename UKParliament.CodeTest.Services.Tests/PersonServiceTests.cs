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
            FirstName = "Bob",
            LastName = "Smith"
        };
        
        var fakePersonRepo = A.Fake<IPersonRepository>();
        
        A.CallTo(() => fakePersonRepo.GetPerson(1)).Returns(fakePerson);
        
        var service = new PersonService(fakePersonRepo);
        
    }
}