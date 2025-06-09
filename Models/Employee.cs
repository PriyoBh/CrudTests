using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CrudAPITests.Models;

public class Employee
{
    [JsonProperty("_id", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? EmployeeId { get; set; }
    [Required]
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string DateOfBirth { get; set; } = String.Empty;
    public string? StartDate { get; set; }
    public string? Department { get; set; }
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? Address { get; set; }
    public string? BaseSalary { get; set; }
}