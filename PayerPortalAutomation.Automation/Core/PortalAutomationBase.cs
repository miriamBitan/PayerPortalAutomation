using Microsoft.Playwright;

namespace PayerPortalAutomation.Automation.Core;

public abstract class PortalAutomationBase
{
    protected async Task<IPage> CreatePageAsync()
    {
        var playwright = await Playwright.CreateAsync();

        var browser = await playwright.Chromium.LaunchAsync(
            new BrowserTypeLaunchOptions
            {
                Headless = false
            });

        return await browser.NewPageAsync();
    }

    protected async Task LoginAsync(
        IPage page,
        string url,
        string username,
        string password)
    {
        await page.GotoAsync("https://gateway.agreeablegrass-c40c311b.eastus.azurecontainerapps.io");

        // מחכים עד שהאלמנט עם class 'login-box' יהיה נוכח
        var loginBox = await page.WaitForSelectorAsync(".login-box", new PageWaitForSelectorOptions
        {
            Timeout = 100000 // זמן מקסימום להמתנה במילישניות
        });

        if (loginBox != null)
        {
            Console.WriteLine("Login box נמצא!");
        }
        else
        {
            Console.WriteLine("לא נמצא login box בזמן ההמתנה.");
        }

        await page.FillAsync("#username-field", username);

        await page.FillAsync("#password-field", password);

        await page.ClickAsync("button[type=submit]");
    }
}