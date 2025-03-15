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
    public async Task<ActionResult<PersonViewModel>> GetById(int id)
    {
        var person = await _personService.GetPersonByIdAsync(id);
        
        if(person == null)
            return NotFound();
        
        return Ok(new PersonViewModel()
        {
            Id = person.Id,
            LastName = person.LastName,
            FirstName = person.FirstName,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Add(PersonViewModel newPersonViewModel)
    {
        var personToAdd = new Person()
        {
            FirstName = newPersonViewModel.FirstName,
            LastName = newPersonViewModel.LastName
        };
        
        var addedPerson = await _personService.AddPersonAsync(personToAdd);

        var returnViewModel = new PersonViewModel()
        {
            Id = addedPerson.Id,
            FirstName = addedPerson.FirstName,
            LastName = addedPerson.LastName
        };
        
        return CreatedAtAction(nameof(GetById), new { id = addedPerson.Id }, returnViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var people = await _personService.GetAllAsync();
        
        var peopleModels = people.Select(p => new PersonViewModel
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName
        });

        return Ok(peopleModels);
    }
}