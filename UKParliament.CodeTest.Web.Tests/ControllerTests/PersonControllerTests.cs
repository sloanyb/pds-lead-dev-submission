using FakeItEasy;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;

namespace UKParliament.CodeTest.Web.Tests.ControllerTests;

public class PersonControllerTests
{
    [Fact]
    public void WhenPersonExists_ThenPersonIsReturned()
    {
        // Note to reviewer: I switched from Moq to FakeItEasy due to the Sponsorlink controversy
        // however am familiar with Moq if that's a library used at Parliament Digital Service. 
        
        var personService = A.Fake<IPersonService>();
        
        // Need the personService to return a Person so I can mock it for this unit test, but I will
        // park this and go do that test then come back to it.
        
        var controller = new PersonController();
        var result = controller.GetById(1);

        Assert.NotNull(result);
        Assert.NotNull(result.Value);
        Assert.Equal("Bloggs", result.Value.LastName);
        Assert.Equal("Joe", result.Value.FirstName);
    }
}