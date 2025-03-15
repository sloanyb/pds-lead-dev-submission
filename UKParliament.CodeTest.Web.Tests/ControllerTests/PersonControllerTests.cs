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
    public async Task WhenPersonExists_ThenPersonIsReturned()
    {
        var personService = A.Fake<IPersonService>();

        var person = new Person()
        {
            Id = 1,
            FirstName = "Joe",
            LastName = "Bloggs"
        };

        A.CallTo(() => personService.GetPersonByIdAsync(1)).Returns(Task.FromResult(person));

        var controller = new PersonController(personService);
        var result = await controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var personViewModel = Assert.IsType<PersonViewModel>(okResult.Value);

        Assert.NotNull(personViewModel);

        Assert.Equal("Bloggs", personViewModel.LastName);
        Assert.Equal("Joe", personViewModel.FirstName);
    }

    [Fact]
    public async Task WhenPersonDoesNotExist_ThenCorrectNotFoundResult()
    {
        var personService = A.Fake<IPersonService>();
        A.CallTo(() => personService.GetPersonByIdAsync(2)).Returns(Task.FromResult<Person?>(null));

        var controller = new PersonController(personService);
        var result = await controller.GetById(2);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task WhenValidPersonIsAdded_ReturnsCreatedAtActionResult()
    {
        var personService = A.Fake<IPersonService>();
        
        A.CallTo(() => personService.AddPersonAsync(A<Person>.Ignored))
            .ReturnsLazily((Person p) =>
            {
                p.Id = 1;
                return Task.FromResult(p);
            });
        var newPersonViewModel = new PersonViewModel
        {
            FirstName = "Alice",
            LastName = "Smith"
        };

        var controller = new PersonController(personService);

        var result = await controller.Add(newPersonViewModel);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetById", createdResult.ActionName);

        var returnedViewModel = Assert.IsType<PersonViewModel>(createdResult.Value);
        Assert.Equal(1, returnedViewModel.Id);
        Assert.Equal("Alice", returnedViewModel.FirstName);
        Assert.Equal("Smith", returnedViewModel.LastName);

        // Additionally, verify that AddPerson was indeed called once with a matching person.
        A.CallTo(() => personService.AddPersonAsync(A<Person>.That.Matches(
                p => p.FirstName == "Alice" && p.LastName == "Smith")))
            .MustHaveHappenedOnceExactly();
    }
}