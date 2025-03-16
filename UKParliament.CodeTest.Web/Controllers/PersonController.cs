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
    public PersonController(IPersonService personService) => _personService = personService;

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonGetViewModel>> GetById(int id)
    {
        var person = await _personService.GetPersonByIdAsync(id);
            
        if (person == null)
            return NotFound();
            
        var viewModel = new PersonGetViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            Department = new PersonGetDepartmentViewModel
            {
                Id = person.Department.Id,
                DepartmentName = person.Department.Name
            }
        };
            
        return Ok(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Add(PersonAddViewModel newPersonUpdateViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var personToAdd = new Person
        {
            FirstName = newPersonUpdateViewModel.FirstName!,
            LastName = newPersonUpdateViewModel.LastName!,
            DateOfBirth = newPersonUpdateViewModel.DateOfBirth!.Value,
            DepartmentId = newPersonUpdateViewModel.DepartmentId!.Value
        };
            
        var addedPerson = await _personService.AddPersonAsync(personToAdd);
            
        var returnViewModel = new PersonGetViewModel
        {
            Id = addedPerson.Id,
            FirstName = addedPerson.FirstName,
            LastName = addedPerson.LastName,
            DateOfBirth = addedPerson.DateOfBirth,
            Department = new PersonGetDepartmentViewModel
            {
                Id = addedPerson.Department.Id,
                DepartmentName = addedPerson.Department.Name
            }
        };
            
        return CreatedAtAction(nameof(GetById), new { id = addedPerson.Id }, returnViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var people = await _personService.GetAllAsync();
            
        var personViewModels = people.Select(p => new PersonGetViewModel
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            DateOfBirth = p.DateOfBirth,
            Department = new PersonGetDepartmentViewModel
            {
                Id = p.Department.Id,
                DepartmentName = p.Department.Name
            }
        });
            
        return Ok(personViewModels);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PersonGetViewModel>> Update(int id, PersonUpdateViewModel personUpdateViewModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var personToUpdate = new Person
        {
            Id = id,
            FirstName = personUpdateViewModel.FirstName!,
            LastName = personUpdateViewModel.LastName!,
            DateOfBirth = personUpdateViewModel.DateOfBirth!.Value,
            DepartmentId = personUpdateViewModel.DepartmentId!.Value
        };
            
        var updatedPerson = await _personService.UpdatePersonAsync(personToUpdate);
            
        var updatedViewModel = new PersonGetViewModel
        {
            Id = updatedPerson.Id,
            FirstName = updatedPerson.FirstName,
            LastName = updatedPerson.LastName,
            DateOfBirth = updatedPerson.DateOfBirth,
            Department = new PersonGetDepartmentViewModel
            {
                Id = updatedPerson.Department.Id,
                DepartmentName = updatedPerson.Department.Name
            }
        };
            
        return Ok(updatedViewModel);
    }
}