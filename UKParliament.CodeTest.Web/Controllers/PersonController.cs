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
            
        // Todo: Mappers! I note the requirement to move mapping logic out of the controller
        // I have just run out of time.
        
        // My thoughts are that I would have an IModelMapper interface, with a ModelMapper
        // implementation setup in DI.
        
        // It would have methods something like:
        // Person MapToPerson(PersonUpdateViewModel input)
        // Person MapToPerson(PersonAddViewModel input)
        // PersonGetViewModel MapToPersonGetViewModel(Person input)
        
        // These would have separate tests around them to ensure all mapping was successful
        // and be used throughout the controller.
        
        // Unfortunately I have run out of time to spend but would get this done with perhaps
        // another 30-45 minutes including full suite of unit tests.
        
        // Therefore all mapping is manually done in the controller although is covered with unit
        // tests; so whilst not ideal, is tested. As an example if you try and change a mapping below
        // and run the tests, it will fail.
        
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