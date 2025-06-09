using CrudAPITests.Models;
using DefaultNamespace;
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
        
        var employeeList = await ApiHelper.GetRestResponse<List<Employee>>("employees");
        employeeList.Count.Should().BeGreaterThan(0);
    }
}