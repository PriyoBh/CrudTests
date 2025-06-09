# Installation 

1. Clone the repository
2. Run command - ```dotnet build```
3. Run tests - ```dotnet test --logger "trx;LogFileName=TestResult.trx```

# Coverage and approach

All CRUD operations have been covered with resource - Employees. Playwright has been integrated to go the website and fetch the api url and store in a text file - keyLookUp.txt This will happen only once when the test suite is run. All employee data created during the test suite will be deleted once all the tests are run. This makesevery test suite run self-contained. The tests themselves are also self-contained meaning they generate the test data they need for the scenario.

The test suite has been built using Restsharp and Playwright.NET

# Scope of improvement

The fetching of the API could be explored to be retrieved by making an API call itself instead of going via UI. The api endpoint itself could be checked to see if it is still active or not and no of calls made so far. Then a fresh url can be retrieved.

Once an API project grows up it might make more sense to create client-services for specific micro-services. IHttpClientFactory can be used to build more scalable services.