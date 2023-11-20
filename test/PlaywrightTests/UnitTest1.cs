using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    [TestFixture]
    public class Tests
    {
        //? How to run the test
        // pwsh bin/Debug/net7.0/playwright.ps1 codegen http://localhost:5273/

        private IBrowserContext _context;
        private IPage page;

        // Set your own password in the environment variable PASSWORD (security reason)
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
        public async Task AppHasPlaywrightTest()
        {
            await page.GotoAsync("http://localhost:5273/");
            await Task.Delay(1000);

            await page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
            await Task.Delay(1000);

            await page.GetByLabel("Username or email address").ClickAsync();
            await Task.Delay(1000);

            await page.GetByLabel("Username or email address").FillAsync("PhiVaGoo@gmail.com");
            await Task.Delay(1000);

            await page.GetByLabel("Username or email address").PressAsync("Tab");
            await Task.Delay(1000);

            await page.GetByLabel("Password").FillAsync(password);
            await Task.Delay(1000);

            await page.GetByLabel("Password").PressAsync("Enter");
            await Task.Delay(4000);

            await page.GetByRole(AriaRole.Link, new() { Name = "SeedDB" }).ClickAsync();
            await Task.Delay(1000);

            void page_Dialog_EventHandler(object sender, IDialog dialog)
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                dialog.DismissAsync();
                page.Dialog -= page_Dialog_EventHandler;
            }
            page.Dialog += page_Dialog_EventHandler;
            await page.GetByRole(AriaRole.Button, new() { Name = "Seed DB2" }).ClickAsync();

            await Task.Delay(2000);

            await page.Locator("li").Filter(new() { HasText = "HelgeCPH 12-01-1992 04:00:00 Follows ondfisk Follow 0 0" }).GetByRole(AriaRole.Button).First.ClickAsync();
            await Task.Delay(1000);
            await page.EvaluateAsync("window.scrollBy(0, 300)");
            await Task.Delay(2000);

            await page.Locator("li").Filter(new() { HasText = "Tien197 12-01-1991 06:00:00 Follows HelgeCPH & ondfisk Follow 0 0" }).GetByRole(AriaRole.Button).First.ClickAsync();
            await Task.Delay(1000);
            await page.EvaluateAsync("window.scrollBy(0, 300)");
            await Task.Delay(2000);

            await page.GetByText("Home Privacy User-Timeline Profile SeedDB").ClickAsync();
            await Task.Delay(1000);
            await page.EvaluateAsync("window.scrollBy(0, 300)");
            await Task.Delay(2000);

            await page.Locator("li").Filter(new() { HasText = "HelgeCPH 12-01-1992 04:00:00 Follows ondfisk Unfollow 0 0" }).GetByRole(AriaRole.Button).First.ClickAsync();
            await Task.Delay(1000);
            await page.EvaluateAsync("window.scrollBy(0, 300)");
            await Task.Delay(2000);

            await page.Locator("li").Filter(new() { HasText = "Tien197 12-01-1991 06:00:00 Follows HelgeCPH & ondfisk Unfollow 0 0" }).Locator("#author").ClickAsync();
            await Task.Delay(1000);
            await page.EvaluateAsync("window.scrollBy(0, 300)");
            await Task.Delay(2000);

            await page.GetByRole(AriaRole.Button, new() { Name = "Unfollow" }).First.ClickAsync();
            await Task.Delay(1000);
            await page.EvaluateAsync("window.scrollBy(0, 300)");
            await Task.Delay(3000);

            await page.GetByText("Home Privacy User-Timeline Profile SeedDB").ClickAsync();
            await Task.Delay(1000);
            await page.EvaluateAsync("window.scrollBy(0, 300)");
            await Task.Delay(2000);

        }
    }
}
