﻿namespace UKParliament.CodeTest.Data.Model;

public class Department
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public ICollection<Person> People { get; set; }
}
