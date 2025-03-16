using FakeItEasy;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Data.Repository;
using Xunit;

namespace UKParliament.CodeTest.Services.Tests
{
    public class PersonServiceTests
    {
        [Fact]
        public async Task PersonService_WhenPersonExists_ReturnsPerson()
        {
            var fakePerson = new Person
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Bloggs",
                DateOfBirth = new DateOnly(1980, 1, 1),
                DepartmentId = 1,
                Department = new Department { Id = 1, Name = "Sales" }
            };

            var fakePersonRepo = A.Fake<IPersonRepository>();
            A.CallTo(() => fakePersonRepo.GetPersonAsync(1)).Returns(Task.FromResult(fakePerson));
            var service = new PersonService(fakePersonRepo);
            var person = await service.GetPersonByIdAsync(1);
            Assert.NotNull(person);
            Assert.Equal("Joe", person.FirstName);
            Assert.Equal("Bloggs", person.LastName);
            Assert.Equal(1, person.Id);
            Assert.Equal(new DateOnly(1980, 1, 1), person.DateOfBirth);
            Assert.Equal(1, person.DepartmentId);
            Assert.NotNull(person.Department);
            Assert.Equal("Sales", person.Department.Name);
        }

        [Fact]
        public async Task PersonService_AddPerson_CallsRepositoryAddPerson()
        {
            var fakePersonRepo = A.Fake<IPersonRepository>();
            var service = new PersonService(fakePersonRepo);
            var newPerson = new Person
            {
                FirstName = "Alice",
                LastName = "Smith",
                DateOfBirth = new DateOnly(1990, 5, 15),
                DepartmentId = 2,
                Department = new Department { Id = 2, Name = "Marketing" }
            };
            var savedPerson = await service.AddPersonAsync(newPerson);
            A.CallTo(() => fakePersonRepo.AddPersonAsync(newPerson)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PersonService_GetAllAsync_ReturnsAllPeople()
        {
            var fakePeople = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Smith",
                    DateOfBirth = new DateOnly(1990, 1, 1),
                    DepartmentId = 1,
                    Department = new Department { Id = 1, Name = "Sales" }
                },
                new Person
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "Jones",
                    DateOfBirth = new DateOnly(1985, 2, 2),
                    DepartmentId = 2,
                    Department = new Department { Id = 2, Name = "Marketing" }
                },
                new Person
                {
                    Id = 3,
                    FirstName = "Charlie",
                    LastName = "Brown",
                    DateOfBirth = new DateOnly(1978, 3, 3),
                    DepartmentId = 3,
                    Department = new Department { Id = 3, Name = "Finance" }
                }
            };

            var fakePersonRepo = A.Fake<IPersonRepository>();
            A.CallTo(() => fakePersonRepo.GetAllAsync()).Returns(Task.FromResult<IEnumerable<Person>>(fakePeople));
            var service = new PersonService(fakePersonRepo);
            var result = await service.GetAllAsync();
            Assert.Equal(3, result.Count());
            Assert.Contains(result, p => p.Id == 1 && p.FirstName == "Alice" && p.LastName == "Smith" && p.DateOfBirth == new DateOnly(1990, 1, 1) && p.DepartmentId == 1 && p.Department?.Name == "Sales");
            Assert.Contains(result, p => p.Id == 2 && p.FirstName == "Bob" && p.LastName == "Jones" && p.DateOfBirth == new DateOnly(1985, 2, 2) && p.DepartmentId == 2 && p.Department?.Name == "Marketing");
            Assert.Contains(result, p => p.Id == 3 && p.FirstName == "Charlie" && p.LastName == "Brown" && p.DateOfBirth == new DateOnly(1978, 3, 3) && p.DepartmentId == 3 && p.Department?.Name == "Finance");
        }

        [Fact]
        public async Task PersonService_UpdatePerson_ReturnsUpdatedPerson()
        {
            var originalPerson = new Person
            {
                Id = 1,
                FirstName = "Joe",
                LastName = "Bloggs",
                DateOfBirth = new DateOnly(1980, 1, 1),
                DepartmentId = 1,
                Department = new Department { Id = 1, Name = "Sales" }
            };
            var updatedPerson = new Person
            {
                Id = 1,
                FirstName = "Joseph",
                LastName = "Bloggs",
                DateOfBirth = new DateOnly(1980, 1, 1),
                DepartmentId = 1,
                Department = new Department { Id = 1, Name = "Sales" }
            };

            var fakePersonRepo = A.Fake<IPersonRepository>();
            A.CallTo(() => fakePersonRepo.UpdatePersonAsync(A<Person>.That.Matches(
                p => p.Id == originalPerson.Id &&
                     p.FirstName == originalPerson.FirstName &&
                     p.LastName == originalPerson.LastName &&
                     p.DateOfBirth == originalPerson.DateOfBirth &&
                     p.DepartmentId == originalPerson.DepartmentId)))
                .Returns(Task.FromResult(updatedPerson));
            var service = new PersonService(fakePersonRepo);
            var result = await service.UpdatePersonAsync(originalPerson);
            Assert.Equal("Joseph", result.FirstName);
            Assert.Equal("Bloggs", result.LastName);
            Assert.Equal(new DateOnly(1980, 1, 1), result.DateOfBirth);
            Assert.Equal(1, result.DepartmentId);
            Assert.NotNull(result.Department);
            Assert.Equal("Sales", result.Department.Name);
            A.CallTo(() => fakePersonRepo.UpdatePersonAsync(A<Person>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
