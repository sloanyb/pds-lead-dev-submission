﻿using UKParliament.CodeTest.Data.Model;

namespace UKParliament.CodeTest.Services;

public interface IPersonService
{
    Task<Person?> GetPersonByIdAsync(int personId);
    Task<Person> AddPersonAsync(Person newPerson);
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person> UpdatePersonAsync(Person person);
}