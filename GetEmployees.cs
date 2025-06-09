using System.Net;
using CrudAPITests.Models;
using CrudAPITests.utils;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;

namespace CrudAPITests;

[TestFixture]
public class GetEmployees
{

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

    //
    
    
}