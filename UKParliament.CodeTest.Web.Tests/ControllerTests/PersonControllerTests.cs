using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.ViewModels;
using Xunit;

namespace UKParliament.CodeTest.Web.Tests.ControllerTests
{
    public class PersonControllerTests
    {
        [Fact]
        public async Task WhenPersonExists_ThenPersonIsReturned()
        {
            var personService = A.Fake<IPersonService>();
            
            var person = new Person
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Bloggs",
                DateOfBirth = new DateOnly(1980, 1, 1),
                DepartmentId = 1,
                Department = new Department { Id = 1, Name = "Sales" }
            };
            A.CallTo(() => personService.GetPersonByIdAsync(1)).Returns(Task.FromResult(person));
            
            var controller = new PersonController(personService);
            var result = await controller.GetById(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var personViewModel = Assert.IsType<PersonGetViewModel>(okResult.Value);
            
            Assert.Equal("Joe", personViewModel.FirstName);
            Assert.Equal("Bloggs", personViewModel.LastName);
            Assert.Equal(1, personViewModel.Id);
            Assert.Equal(new DateOnly(1980, 1, 1), personViewModel.DateOfBirth);
            Assert.Equal(1, personViewModel.Department.Id);
            Assert.Equal("Sales", personViewModel.Department.DepartmentName);
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
                    p.Department = new Department { Id = p.DepartmentId, Name = p.DepartmentId == 1 ? "Sales" : p.DepartmentId == 2 ? "Marketing" : p.DepartmentId == 3 ? "Finance" : "HR" };
                    return Task.FromResult(p);
                });
            
            var newPersonViewModel = new PersonUpdateViewModel
            {
                FirstName = "Alice",
                LastName = "Smith",
                DateOfBirth = new DateOnly(1990, 5, 15),
                DepartmentId = 1
            };
            var controller = new PersonController(personService);
            var result = await controller.Add(newPersonViewModel);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            
            Assert.Equal("GetById", createdResult.ActionName);
            var returnedViewModel = Assert.IsType<PersonGetViewModel>(createdResult.Value);
            
            Assert.Equal(1, returnedViewModel.Id);
            Assert.Equal("Alice", returnedViewModel.FirstName);
            Assert.Equal("Smith", returnedViewModel.LastName);
            Assert.Equal(new DateOnly(1990, 5, 15), returnedViewModel.DateOfBirth);
            Assert.Equal(1, returnedViewModel.Department.Id);
            Assert.Equal("Sales", returnedViewModel.Department.DepartmentName);
            
            A.CallTo(() => personService.AddPersonAsync(A<Person>.That.Matches(
                p => p.FirstName == "Alice" && p.LastName == "Smith" && p.DateOfBirth == new DateOnly(1990, 5, 15) && p.DepartmentId == 1)))
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task WhenGetAllCalled_ReturnsAllPeople()
        {
            var fakePeople = new List<Person>
            {
                new()
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Smith",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    DepartmentId = 1,
                    Department = new Department { Id = 1, Name = "Sales" }
                },
                new()
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Jones",
                    DateOfBirth = new DateOnly(1985, 2, 2),
                    DepartmentId = 2,
                    Department = new Department { Id = 2, Name = "Marketing" }
                }
            };
            var personService = A.Fake<IPersonService>();
            A.CallTo(() => personService.GetAllAsync()).Returns(Task.FromResult<IEnumerable<Person>>(fakePeople));
            
            var controller = new PersonController(personService);
            var result = await controller.GetAllAsync();
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPeopleViewModels = Assert.IsAssignableFrom<IEnumerable<PersonGetViewModel>>(okResult.Value);
            
            Assert.Equal(2, returnedPeopleViewModels.Count());
            Assert.Contains(returnedPeopleViewModels, vm => vm.Id == 1 && vm.FirstName == "Alice" && vm.LastName == "Smith" && vm.DateOfBirth == new DateOnly(1990, 1, 1) && vm.Department.Id == 1 && vm.Department.DepartmentName == "Sales");
            Assert.Contains(returnedPeopleViewModels, vm => vm.Id == 2 && vm.FirstName == "Bob" && vm.LastName == "Jones" && vm.DateOfBirth == new DateOnly(1985, 2, 2) && vm.Department.Id == 2 && vm.Department.DepartmentName == "Marketing");
        }

        [Fact]
        public async Task WhenValidPersonIsUpdated_ReturnsUpdatedPerson()
        {
            var updatedPerson = new Person
            {
                Id = 1,
                FirstName = "Joseph",
                LastName = "Bloggs",
                DateOfBirth = new DateOnly(1980, 1, 1),
                DepartmentId = 1,
                Department = new Department { Id = 1, Name = "Sales" }
            };
            
            var personService = A.Fake<IPersonService>();
            
            A.CallTo(() => personService.UpdatePersonAsync(A<Person>.That.Matches(
                p => p.Id == 1 && p.FirstName == "Joseph" && p.LastName == "Bloggs" && p.DateOfBirth == new DateOnly(1980, 1, 1) && p.DepartmentId == 1)))
                .Returns(Task.FromResult(updatedPerson));
           
            var controller = new PersonController(personService);
            var updateViewModel = new PersonUpdateViewModel
            {
                Id = 1,
                FirstName = "Joseph",
                LastName = "Bloggs",
                DateOfBirth = new DateOnly(1980, 1, 1),
                DepartmentId = 1
            };
            
            var result = await controller.Update(1, updateViewModel);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedViewModel = Assert.IsType<PersonGetViewModel>(okResult.Value);
            
            Assert.Equal(1, returnedViewModel.Id);
            Assert.Equal("Joseph", returnedViewModel.FirstName);
            Assert.Equal("Bloggs", returnedViewModel.LastName);
            Assert.Equal(new DateOnly(1980, 1, 1), returnedViewModel.DateOfBirth);
            Assert.Equal(1, returnedViewModel.Department.Id);
            Assert.Equal("Sales", returnedViewModel.Department.DepartmentName);
        }
    }
}
