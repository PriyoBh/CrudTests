using Microsoft.Playwright.NUnit;

namespace CrudAPITests;

[TestFixture]
public class Setup:PageTest
{
    [Test]
    public async Task SetupUrl()
    {
        await Page.GotoAsync("https://crudcrud.com/");
        var urlInformation = await Page.Locator("xpath=//div[contains(@class, 'endpoint')]").First.InnerTextAsync();
        var fileName = Path.Join(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,"keyLookUp.txt");
        if (File.Exists(fileName))
            File.Delete(fileName);
        await File.WriteAllTextAsync(fileName, $"{urlInformation}");
    }
}