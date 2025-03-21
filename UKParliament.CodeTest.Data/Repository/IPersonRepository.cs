﻿using UKParliament.CodeTest.Data.Model;

namespace UKParliament.CodeTest.Data.Repository;

public interface IPersonRepository
{
    Task<Person?> GetPersonAsync(int personId);
    Task<Person> AddPersonAsync(Person newPerson);
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person> UpdatePersonAsync(Person person);
}