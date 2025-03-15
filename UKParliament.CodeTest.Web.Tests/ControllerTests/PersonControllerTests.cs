using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.ViewModels;
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
        
        var okObjectResult = result.Result as OkObjectResult;
        var personViewModel = okObjectResult.Value as PersonViewModel;
        
        Assert.Equal("Bloggs",personViewModel.LastName);
        Assert.Equal("Joe", personViewModel.FirstName);
    }
}