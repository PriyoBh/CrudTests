using System.Net;
using CrudAPITests.Models;
using CrudAPITests.utils;
using FluentAssertions;
using Microsoft.Playwright;
using Newtonsoft.Json;

namespace CrudAPITests;

[TestFixture]
public class EmployeesCrudOperations
{

    [OneTimeSetUp]
    public static async Task Setup()
    {
        /*using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();
        await page.GotoAsync("https://crudcrud.com/");
        var urlInformation = await page.Locator("xpath=//div[contains(@class, 'endpoint')]").First.InnerTextAsync();
        var fileName = Path.Join(Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName,"keyLookUp.txt");
        if (File.Exists(fileName))
            File.Delete(fileName);
        await File.WriteAllTextAsync(fileName, $"{urlInformation}");*/
    }

    [Test]
    public async Task GetEmployeeDetails()
    {
        var employee = new Employee
        {
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = "01/01/2000",
            Address = "2 Walker Street North Sydney - 2060",
            BaseSalary = "50000",
            Department = "HR",
            Email = "jenny@crud.com",
            JobTitle = "Office Admin",
            Mobile = "0410568789",
            StartDate = "05/01/2018"
        };
        var employeeData = JsonConvert.SerializeObject(employee);
        var response = await ApiHelper.PostRestResponse("employees", employeeData);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var employeeList = await ApiHelper.GetRestResponse<List<Employee>>("employees");
        employeeList.Count.Should().BeGreaterThan(0);
        employeeList.Select(employee => employee.FirstName == "John" && employee.LastName == "Smith").Should()
            .NotBeNull();
    }

    [Test]
    public async Task CreateEmployee()
    {
        var employeesFile = Path.Join((Environment.CurrentDirectory), "Data/Employees.json");
        if (!File.Exists(employeesFile))
            throw new FileNotFoundException("Employees file not found", employeesFile);
        var employeesData = JsonConvert.DeserializeObject<List<Employee>>(await File.ReadAllTextAsync(employeesFile));
        foreach (var employee in employeesData!)
        {
            var employeeData = JsonConvert.SerializeObject(employee);
            var response = await ApiHelper.PostRestResponse("employees", employeeData);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        
        var employeeList = await ApiHelper.GetRestResponse<List<Employee>>("employees");
        
        employeeList.Where(employee => employee.FirstName == "Foggy" && employee.LastName == "Nelson").ToList().Count.Should().Be(1);
        employeeList.Where(employee => employee.FirstName == "Matthew" && employee.LastName == "Murdock").ToList().Count.Should().Be(1);
        employeeList.Where(employee => employee.FirstName == "Karen" && employee.LastName == "Paige").ToList().Count.Should().Be(1);

    }

    [Test]
    public async Task UpdateEmployeeDetails()
    {
        var employee = new Employee
        {
            FirstName = "Wilson",
            LastName = "Fisk",
            DateOfBirth = "01/01/2000",
            Address = "2 Walker Street North Sydney - 2060",
            BaseSalary = "50000",
            Department = "HR",
            Email = "jenny@crud.com",
            JobTitle = "Office Admin",
            Mobile = "0410568789",
            StartDate = "05/01/2018"
        };
        var employeeData = JsonConvert.SerializeObject(employee);
        var response = await ApiHelper.PostRestResponse("employees", employeeData);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var employeeList = await ApiHelper.GetRestResponse<List<Employee>>("employees");
        var employeeId = employeeList
            .Where(item => item is { FirstName: "Wilson", LastName: "Fisk", DateOfBirth: "01/01/2000" })
            .Select(x => x.EmployeeId).First();
        var updatedEmployee = new Employee
        {
            FirstName = "Kingpin",
            LastName = "TheOne",
            DateOfBirth = "01/01/2000",
            Address = "2 Walker Street North Sydney - 2060",
            BaseSalary = "50000",
            Department = "HR",
            Email = "jenny@crud.com",
            JobTitle = "Office Admin",
            Mobile = "0410568789",
            StartDate = "05/01/2018"
        };
        var putResponse =
            await ApiHelper.PutRestResponse($"employees/{employeeId}", JsonConvert.SerializeObject(updatedEmployee));
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updatedEmployeeList = await ApiHelper.GetRestResponse<List<Employee>>("employees");
        updatedEmployeeList.Where(item => item.EmployeeId == employeeId).Select(employeeInfo => employeeInfo.FirstName)
            .First().Should().Be("Kingpin");
    }

    [OneTimeTearDown]
    public async Task DeleteAllData()
    {
        var employeeList = await ApiHelper.GetRestResponse<List<Employee>>("employees");
        var employeeIds = employeeList.Select(employee => employee.EmployeeId).ToList();
        foreach (var employeeId in employeeIds)
        {
            var response = await ApiHelper.DeleteRestResponse($"employees/{employeeId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }


};