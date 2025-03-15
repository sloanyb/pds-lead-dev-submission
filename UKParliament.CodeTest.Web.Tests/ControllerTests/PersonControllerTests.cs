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

        Assert.IsType<OkObjectResult>(result.Result);
        
        var personViewModel = ((OkObjectResult)result.Result).Value as PersonViewModel;
        
        Assert.NotNull(personViewModel);
        
        Assert.Equal("Bloggs",personViewModel.LastName);
        Assert.Equal("Joe", personViewModel.FirstName);
    }
    
    [Fact]
    public void WhenPersonDoesNotExist_ThenCorrectNotFoundResult()
    {
        var personService = A.Fake<IPersonService>();
        A.CallTo(() => personService.GetPersonById(2)).Returns(null);
        
        var controller = new PersonController(personService);
        var result = controller.GetById(2);
        
        Assert.IsType<NotFoundResult>(result.Result);
    }
}