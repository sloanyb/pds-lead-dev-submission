﻿using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Data.Repository;
using Xunit;

namespace UKParliament.CodeTest.Data.Tests;

public class PersonRepositoryTests
{
    [Fact]
    public void GetById_ForExistingPerson_ReturnsPerson()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    
        using (var context = new PersonManagerContext(options))
        {
            context.People.Add(new Person { Id = 1, FirstName = "Bob", LastName = "Smith" });
            context.SaveChanges();
        }
    
        using (var context = new PersonManagerContext(options))
        {
            var repo = new PersonRepository(context);
            var person = repo.GetPerson(1);
        
            Assert.NotNull(person);
            Assert.Equal("Joe", person.FirstName);
        }
    }
}