using Microsoft.Playwright;

namespace CrudAPITests.utils;

public static class PlaywrightHelper
{
    public static async Task CreateApiUrl()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();
        await page.GotoAsync("https://crudcrud.com/");
        var urlInformation = await page.Locator("xpath=//div[contains(@class, 'endpoint')]").First.InnerTextAsync();
        var fileName = Path.Join(Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.FullName,"keyLookUp.txt");
            if (File.Exists(fileName))
                File.Delete(fileName);
        await File.WriteAllTextAsync(fileName, $"{urlInformation}");
    }
    
}