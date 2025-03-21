﻿using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Web.ViewModels;

public class PersonUpdateViewModel
{
    [Required(ErrorMessage = "ID is required")]
    public int? Id { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Date of Birth is required.")]
    public DateOnly? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Department is required.")]
    public int? DepartmentId { get; set; }
}