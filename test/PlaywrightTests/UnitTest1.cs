using System.Text.RegularExpressions;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    [TestFixture]
    public class Tests
    {
        /*
         ? Recording test with Playwright: 
         ! pwsh bin/Debug/net7.0/playwright.ps1 codegen http://localhost:5273/
        */
        private IBrowserContext _context;
        private IPage page;
        // private string email = "PhiVaGoo@gmail.com";
        private string email = Environment.GetEnvironmentVariable("EMAIL");
        private string password = Environment.GetEnvironmentVariable("PASSWORD");
        private int count;
        private bool isExist;

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

        private async Task ClickWriteCheepToggle()
        {
            await page.GetByRole(AriaRole.Button, new() { Name = "Write Cheep" }).ClickAsync(); await Task.Delay(2000);
        }

        [Test]
        public async Task LoginAndSeedDbInitializer2AndFollowUnfollowAuthorsAndCheckUserTimeline()
        {
            // Go to localhost
            await page.GotoAsync("http://localhost:5273/"); await Task.Delay(1000);

            // Sign in github
            await page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync(); await Task.Delay(1000);
            await page.GetByLabel("Username or email address").ClickAsync(); await Task.Delay(1000);
            await page.GetByLabel("Username or email address").FillAsync(email); await Task.Delay(1000);
            await page.GetByLabel("Username or email address").PressAsync("Tab"); await Task.Delay(1000);
            await page.GetByLabel("Password").FillAsync(password); await Task.Delay(1000);
            await page.GetByLabel("Password").PressAsync("Enter"); await Task.Delay(4000);

            // Seed new test data with other author following another different author 
            //      Author (B) follows Author (C) and (D), Author (C) follows Author (D) 
            await page.GetByRole(AriaRole.Link, new() { Name = "SeedDB" }).ClickAsync(); await Task.Delay(2000);
            await page.GetByRole(AriaRole.Button, new() { Name = "Seed DB2" }).ClickAsync(); await Task.Delay(2000);
            ClickWriteCheepToggle();

            // I follow two different authors (B, C)
            await page.Locator("li").Filter(new() { HasText = "HelgeCPH" }).GetByRole(AriaRole.Button).First.ClickAsync(); await Task.Delay(2000);
            await page.EvaluateAsync("window.scrollBy(0, 300)"); await Task.Delay(2000);
            await page.Locator("li").Filter(new() { HasText = "Chirp27" }).GetByRole(AriaRole.Button).First.ClickAsync(); await Task.Delay(2000);
            await page.EvaluateAsync("window.scrollBy(0, 300)"); await Task.Delay(2000);

            // Go into my UserTimeline - both of these authors cheeps should appear in my user-timeline
            await page.GetByText("User-Timeline").ClickAsync(); await Task.Delay(2000);
            await page.EvaluateAsync("window.scrollBy(0, 300)"); await Task.Delay(4000);
            Assert.IsTrue(await page.Locator("text='Chirp27'").CountAsync() > 0, "Author not found");
            Assert.IsTrue(await page.Locator("text='HelgeCPH'").CountAsync() > 0, "Author not found");
            Assert.IsFalse(await page.Locator("text='ondfisk'").IsVisibleAsync());

            // I unfollow one of the author (C) - his cheeps should not appear in my timeline anymore
            await page.Locator("li:has-text('HelgeCPH')").First.GetByRole(AriaRole.Button).GetByText("Unfollow").ClickAsync(); await Task.Delay(2000);
            await page.EvaluateAsync("window.scrollBy(0, 300)"); await Task.Delay(2000);
            Assert.IsFalse(await page.Locator("text='HelgeCPH'").IsVisibleAsync());

            // Go to my timeline of author (B) - his timelime should not display his followers cheeps
            await page.Locator("li:has-text('Chirp27')").First.Locator("#author").ClickAsync(); await Task.Delay(4000);

            ClickWriteCheepToggle();
            await page.EvaluateAsync("window.scrollBy(0, 500)"); await Task.Delay(2000);
            Assert.IsFalse(await page.Locator("text='HelgeCPH'").IsVisibleAsync());
            Assert.IsFalse(await page.Locator("text='ondfisk'").IsVisibleAsync());

            // Unfollowing Author (B) (B's cheeps should not appear in my timeline anymore)
            await page.GetByRole(AriaRole.Button, new() { Name = "Unfollow" }).First.ClickAsync(); await Task.Delay(2000);
            ClickWriteCheepToggle();
            await page.EvaluateAsync("window.scrollBy(0, 1000)"); await Task.Delay(2000);

            // Go to my timeline
            await page.GetByText("User-Timeline").ClickAsync(); await Task.Delay(5000);
            Assert.IsFalse(await page.Locator("text='Chirp27'").IsVisibleAsync());
            Assert.IsFalse(await page.Locator("text='HelgeCPH'").IsVisibleAsync());

            // Send a cheep
            await page.GetByPlaceholder("Write your cheep here!").ClickAsync(); await Task.Delay(2000);
            await page.GetByPlaceholder("Write your cheep here!").FillAsync("🎉 Playwright test done 🎉"); await Task.Delay(2000);
            await page.GetByRole(AriaRole.Button, new() { Name = "Chirp!" }).ClickAsync(); await Task.Delay(5000);
            await page.GetByPlaceholder("Write your cheep here!").ClickAsync(); await Task.Delay(2000);
            await page.GetByPlaceholder("Write your cheep here!").FillAsync("Signing out..."); await Task.Delay(2000);
            await page.GetByRole(AriaRole.Button, new() { Name = "Chirp!" }).ClickAsync(); await Task.Delay(5000);

            // Sign out
            await page.GetByRole(AriaRole.Button, new() { Name = "Sign out" }).ClickAsync(); await Task.Delay(5000);
        }
    }
}