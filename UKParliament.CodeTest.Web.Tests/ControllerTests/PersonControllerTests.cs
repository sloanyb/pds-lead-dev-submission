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

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var personViewModel = Assert.IsType<PersonViewModel>(okResult.Value);

        Assert.NotNull(personViewModel);

        Assert.Equal("Bloggs", personViewModel.LastName);
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

    [Fact]
    public void WhenValidPersonIsAdded_ReturnsCreatedAtActionResult()
    {
        var personService = A.Fake<IPersonService>();
        var newPersonViewModel = new PersonViewModel
        {
            FirstName = "Alice",
            LastName = "Smith"
        };

        var controller = new PersonController(personService);

        var result = controller.Add(newPersonViewModel);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetById", createdResult.ActionName);

        var returnedViewModel = Assert.IsType<PersonViewModel>(createdResult.Value);
        Assert.Equal(2, returnedViewModel.Id);
        Assert.Equal("Alice", returnedViewModel.FirstName);
        Assert.Equal("Smith", returnedViewModel.LastName);

        // Additionally, verify that AddPerson was indeed called once with a matching person.
        A.CallTo(() => personService.AddPerson(A<Person>.That.Matches(
                p => p.FirstName == "Alice" && p.LastName == "Smith")))
            .MustHaveHappenedOnceExactly();
    }
}