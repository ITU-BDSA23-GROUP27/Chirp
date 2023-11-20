using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    [TestFixture]
    public class Tests
    {
        //? Recording test with Playwright
        //# pwsh bin/Debug/net7.0/playwright.ps1 codegen http://localhost:5273/

        private IBrowserContext _context;
        private IPage page;
        private string account1 = "PhiVaGoo@gmail.com";
        private string password = Environment.GetEnvironmentVariable("PASSWORD");

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });

            _context = await browser.NewContextAsync();
            page = await _context.NewPageAsync();

            await page.SetViewportSizeAsync(1920, 1080);
        }

        [Test]
        public async Task LoginAndSeedDbInitializer2AndFollowUnfollowAuthorsAndCheckUserTimeline()
        {
            await page.GotoAsync("http://localhost:5273/");
            await Task.Delay(1000);

            await page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
            await Task.Delay(1000);

            await page.GetByLabel("Username or email address").ClickAsync();
            await Task.Delay(1000);

            await page.GetByLabel("Username or email address").FillAsync(account1);
            await Task.Delay(1000);

            await page.GetByLabel("Username or email address").PressAsync("Tab");
            await Task.Delay(1000);

            await page.GetByLabel("Password").FillAsync(password);
            await Task.Delay(1000);

            await page.GetByLabel("Password").PressAsync("Enter");
            await Task.Delay(4000);

            await page.GetByRole(AriaRole.Link, new() { Name = "SeedDB" }).ClickAsync();
            await Task.Delay(1000);

            await page.GetByRole(AriaRole.Button, new() { Name = "Seed DB2" }).ClickAsync();
            await Task.Delay(2000);

            // await page.Locator("li").Filter(new() { HasText = "HelgeCPH 12-01-1992 04:00:00 Follows ondfisk Follow 0 0" }).GetByRole(AriaRole.Button).First.ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Write Cheep" }).ClickAsync();
            await Task.Delay(2000);
            await page.Locator("li").Filter(new() { HasText = "HelgeCPH" }).GetByRole(AriaRole.Button).First.ClickAsync();
            await Task.Delay(2000);

            await page.GetByRole(AriaRole.Button, new() { Name = "Write Cheep" }).ClickAsync();
            await Task.Delay(2000);
            await page.Locator("li").Filter(new() { HasText = "Tien197" }).GetByRole(AriaRole.Button).First.ClickAsync();
            await Task.Delay(2000);

            await page.GetByText("User-Timeline").ClickAsync();
            await Task.Delay(2000);

            await page.GetByRole(AriaRole.Button, new() { Name = "Write Cheep" }).ClickAsync();
            await Task.Delay(2000);
            await page.Locator("li").Filter(new() { HasText = "HelgeCPH 12-01-1992 04:00:00 Follows ondfisk Unfollow 0 0" }).GetByRole(AriaRole.Button).First.ClickAsync();
            await Task.Delay(2000);

            await page.GetByRole(AriaRole.Button, new() { Name = "Write Cheep" }).ClickAsync();
            await Task.Delay(2000);
            await page.Locator("li").Filter(new() { HasText = "Tien197 12-01-1991 06:00:00 Follows HelgeCPH & ondfisk Unfollow 0 0" }).Locator("#author").ClickAsync();
            await Task.Delay(2000);

            await page.GetByRole(AriaRole.Button, new() { Name = "Write Cheep" }).ClickAsync();
            await Task.Delay(2000);
            await page.GetByRole(AriaRole.Button, new() { Name = "Unfollow" }).First.ClickAsync();
            await Task.Delay(2000);

            await page.GetByText("User-Timeline").ClickAsync();
            await Task.Delay(5000);
            // await page.EvaluateAsync("window.scrollBy(0, 300)");

        }
    }
}
