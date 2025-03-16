namespace UKParliament.CodeTest.Web.ViewModels;

public class PersonGetViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public PersonGetDepartmentViewModel Department { get; set; }
}