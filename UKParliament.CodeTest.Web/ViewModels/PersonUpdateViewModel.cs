namespace UKParliament.CodeTest.Web.ViewModels;

public class PersonUpdateViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public int DepartmentId { get; set; }
}