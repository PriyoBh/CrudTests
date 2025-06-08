using CrudAPITests.Models;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;

namespace CrudAPITests;

[TestFixture]
public class GetEmployees
{
    private string baseUrl;

    [OneTimeSetUp]
    public void GetUrl()
    {
        var fileName = Path.Join(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,"keyLookUp.txt");
        if (!File.Exists(fileName))
            throw new FileNotFoundException();
        baseUrl = File.ReadAllText(fileName);
    }
    
    [Test]
    public async Task GetEmployeeDetails()
    {
        var options = new RestClientOptions(baseUrl) {
            
        };
        var client = new RestClient(options);
        var request = new RestRequest("employees");
  
        var employeeContent = client.ExecuteGetAsync(request).Result.Content;
        var employeeList = JsonConvert.DeserializeObject<List<Employee>>(employeeContent);
        employeeList.Count.Should().BeGreaterThan(0);
    }
}