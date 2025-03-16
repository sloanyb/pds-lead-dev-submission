using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data.Model;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [Route("{id:int}")]
    [HttpGet]
    public async Task<ActionResult<PersonUpdateViewModel>> GetById(int id)
    {
        var person = await _personService.GetPersonByIdAsync(id);
        
        if(person == null)
            return NotFound();
        
        return Ok(new PersonUpdateViewModel()
        {
            Id = person.Id,
            LastName = person.LastName,
            FirstName = person.FirstName,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Add(PersonUpdateViewModel newPersonUpdateViewModel)
    {
        var personToAdd = new Person()
        {
            FirstName = newPersonUpdateViewModel.FirstName,
            LastName = newPersonUpdateViewModel.LastName,
            DepartmentId = newPersonUpdateViewModel.DepartmentId
        };
        
        var addedPerson = await _personService.AddPersonAsync(personToAdd);

        var returnViewModel = new PersonUpdateViewModel()
        {
            Id = addedPerson.Id,
            FirstName = addedPerson.FirstName,
            LastName = addedPerson.LastName,
            DepartmentId = addedPerson.DepartmentId
        };
        
        return CreatedAtAction(nameof(GetById), new { id = addedPerson.Id }, returnViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var people = await _personService.GetAllAsync();
        
        var peopleModels = people.Select(p => new PersonUpdateViewModel
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName
        });

        return Ok(peopleModels);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<PersonUpdateViewModel>> Update(int id, PersonUpdateViewModel personUpdateViewModel)
    {
        var personToUpdate = new Person
        {
            Id = id,
            FirstName = personUpdateViewModel.FirstName,
            LastName = personUpdateViewModel.LastName,
            DepartmentId = personUpdateViewModel.DepartmentId
        };

        var updatedPerson = await _personService.UpdatePersonAsync(personToUpdate);
        
        var updatedViewModel = new PersonUpdateViewModel
        {
            Id = id,
            FirstName = updatedPerson.FirstName,
            LastName = updatedPerson.LastName,
            DepartmentId = updatedPerson.DepartmentId
        };

        return Ok(updatedViewModel);
    }
}