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
        var personViewModel = Assert.IsType<PersonUpdateViewModel>(okResult.Value);

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
        var newPersonViewModel = new PersonUpdateViewModel
        {
            FirstName = "Alice",
            LastName = "Smith"
        };

        var controller = new PersonController(personService);

        var result = await controller.Add(newPersonViewModel);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetById", createdResult.ActionName);

        var returnedViewModel = Assert.IsType<PersonUpdateViewModel>(createdResult.Value);
        Assert.Equal(1, returnedViewModel.Id);
        Assert.Equal("Alice", returnedViewModel.FirstName);
        Assert.Equal("Smith", returnedViewModel.LastName);

        // Additionally, verify that AddPerson was indeed called once with a matching person.
        A.CallTo(() => personService.AddPersonAsync(A<Person>.That.Matches(
                p => p.FirstName == "Alice" && p.LastName == "Smith")))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task WhenGetAllCalled_ReturnsAllPeople()
    {
        var fakePeople = new List<Person>
        {
            new Person { Id = 1, FirstName = "Alice", LastName = "Smith" },
            new Person { Id = 2, FirstName = "Bob", LastName = "Jones" }
        };

        var personService = A.Fake<IPersonService>();
        A.CallTo(() => personService.GetAllAsync())
            .Returns(Task.FromResult<IEnumerable<Person>>(fakePeople));

        var controller = new PersonController(personService);

        var result = await controller.GetAllAsync();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var viewModelList = Assert.IsAssignableFrom<IEnumerable<PersonUpdateViewModel>>(okResult.Value);

        Assert.Equal(2, viewModelList.Count());
        Assert.Contains(viewModelList, vm => vm.Id == 1 && vm.FirstName == "Alice" && vm.LastName == "Smith");
        Assert.Contains(viewModelList, vm => vm.Id == 2 && vm.FirstName == "Bob" && vm.LastName == "Jones");
    }

    [Fact]
    public async Task WhenValidPersonIsUpdated_ReturnsUpdatedPerson()
    {
        var updatedPerson = new Person { Id = 1, FirstName = "Joseph", LastName = "Bloggs" };

        var personService = A.Fake<IPersonService>();

        A.CallTo(() => personService.UpdatePersonAsync(A<Person>.That.Matches(
                p => p.Id == 1 && p.FirstName == "Joseph" && p.LastName == "Bloggs")))
            .Returns(Task.FromResult(updatedPerson));

        var controller = new PersonController(personService);
        
        var updateViewModel = new PersonUpdateViewModel
        {
            Id = 1,
            FirstName = "Joseph",
            LastName = "Bloggs"
        };

        var result = await controller.Update(1, updateViewModel);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedViewModel = Assert.IsType<PersonUpdateViewModel>(okResult.Value);
        
        Assert.Equal(1, returnedViewModel.Id);
        Assert.Equal("Joseph", returnedViewModel.FirstName);
        Assert.Equal("Bloggs", returnedViewModel.LastName);
    }
}
