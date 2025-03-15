using FakeItEasy;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;

namespace UKParliament.CodeTest.Web.Tests.ControllerTests;

public class PersonControllerTests
{
    [Fact]
    public void WhenPersonExists_ThenPersonIsReturned()
    {
        var personService = A.Fake<IPersonService>();

        var person = new Person()
        {
            Id = 1,
            FirstName = "Joe",
            LastName = "Bloggs"
        };
        
        A.CallTo(() => personService.GetPersonById(1)).Returns(person);
        
        var controller = new PersonController(personService);
        
        var result = controller.GetById(1);

        Assert.NotNull(result);
        Assert.NotNull(result.Value);
        Assert.Equal("Bloggs", result.Value.LastName);
        Assert.Equal("Joe", result.Value.FirstName);
    }
}