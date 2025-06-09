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
        await PlaywrightHelper.CreateApiUrl();
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
        
        var employeeNames = employeeList.Select(employee => employee.FirstName).ToList();
        employeeNames.Should().Contain(["Foggy", "Matthew", "Karen"]);
        
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

    [Test]
    public async Task DeleteEmployee()
    {
        var employeeBenjamin = new Employee
        {
            FirstName = "Benjamin",
            LastName = "Ulrich",
            DateOfBirth = "01/01/2000",
            Address = "2 Walker Street North Sydney - 2060",
            BaseSalary = "50000",
            Department = "HR",
            Email = "ben@crud.com",
            JobTitle = "Office Admin",
            Mobile = "0410568789",
            StartDate = "05/01/2018"
        };
        var employeeDataBen = JsonConvert.SerializeObject(employeeBenjamin);
        var createResponse = await ApiHelper.PostRestResponse("employees", employeeDataBen);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var employeeWilson = new Employee
        {
            FirstName = "Wilson",
            LastName = "Fisk",
            DateOfBirth = "01/01/2000",
            Address = "2 Walker Street North Sydney - 2060",
            BaseSalary = "50000",
            Department = "HR",
            Email = "wilson@crud.com",
            JobTitle = "Office Admin",
            Mobile = "0410568789",
            StartDate = "05/01/2018"
        };
        var employeeDataWilson = JsonConvert.SerializeObject(employeeWilson);
        createResponse = await ApiHelper.PostRestResponse("employees", employeeDataWilson);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var employeeList = await ApiHelper.GetRestResponse<List<Employee>>("employees");
        var employeeId = employeeList
            .Where(item => item is { FirstName: "Benjamin", LastName: "Ulrich", DateOfBirth: "01/01/2000" })
            .Select(x => x.EmployeeId).First();
        var deleteResponse = await ApiHelper.DeleteRestResponse($"employees/{employeeId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);     
        employeeList = await ApiHelper.GetRestResponse<List<Employee>>("employees");
        employeeList.Select(employee => employee.FirstName).ToList().Should().NotContain("Benjamin");
        
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