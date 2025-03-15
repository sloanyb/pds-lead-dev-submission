using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Model;
using Xunit;

namespace UKParliament.CodeTest.Services.Tests;

public class PersonServiceTests
{
    private static PersonManagerContext GetFakeContext()
    {
        var fakePeople = new List<Person>
        {
            new() { Id = 1, FirstName = "Alice", LastName = "Smith" },
            new() { Id = 2, FirstName = "Bob", LastName = "Jones" }
        }.AsQueryable();

        var fakePeopleDbSet = A.Fake<DbSet<Person>>(options => options.Implements(typeof(IQueryable<Person>)));

        A.CallTo(() => ((IQueryable<Person>)fakePeopleDbSet).Provider).Returns(fakePeople.Provider);
        A.CallTo(() => ((IQueryable<Person>)fakePeopleDbSet).Expression).Returns(fakePeople.Expression);
        A.CallTo(() => ((IQueryable<Person>)fakePeopleDbSet).ElementType).Returns(fakePeople.ElementType);
        A.CallTo(() => ((IQueryable<Person>)fakePeopleDbSet).GetEnumerator()).Returns(fakePeople.GetEnumerator());

        var fakeContext = A.Fake<PersonManagerContext>();

        A.CallTo(() => fakeContext.People).Returns(fakePeopleDbSet);
        
        return fakeContext;
    }
    
    [Fact]
    public void PersonService_Get_GetsPerson()
    {
        var fakeContext = GetFakeContext();

        var service = new PersonService(fakeContext);
    }
}