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
        //! Note: setup your own environment variables, or hardcode your email and password here to temporarily run the test
        private string email = Environment.GetEnvironmentVariable("EMAIL");
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

        private async Task ClickWriteCheepToggle(IPage page)
        {
            await Task.Delay(2000);
            await page.GetByRole(AriaRole.Button, new() { Name = "Write Cheep" }).ClickAsync(); 
            await Task.Delay(2000);
        }

        private async Task ScrollDown(IPage page)
        {
            await Task.Delay(2000);
            await page.EvaluateAsync("window.scrollBy(0, 500)");
            await Task.Delay(2000);
        }

        private async Task DisplayMessage(IPage page, string message)
        {
            await Task.Delay(1000);
            await page.GetByPlaceholder("Write your cheep here!").ClickAsync();
            await Task.Delay(1000);
            await page.GetByPlaceholder("Write your cheep here!").FillAsync($"{message}");
            await Task.Delay(2000);
        }


        [Test]
        public async Task LoginAndSeedDbInitializer2AndFollowUnfollowUsersAndCheckUserTimeline()
        {
            // Go to localhost 
            await page.GotoAsync("http://localhost:5273/"); await Task.Delay(1000);
            Assert.IsTrue(await page.Locator("text='Sign in'").IsVisibleAsync());
            Assert.IsFalse(await page.Locator("text='Sign out'").IsVisibleAsync());
            Assert.IsFalse(await page.Locator("text='About me'").IsVisibleAsync());
            Assert.IsFalse(await page.Locator("text='User timeline'").IsVisibleAsync());
            Assert.IsFalse(await page.Locator("textarea").IsVisibleAsync(), "Textarea should not be visible on the page");

            // Sign in github
            await page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();                             await Task.Delay(1000);
            await page.GetByLabel("Username or email address").ClickAsync();                                            await Task.Delay(1000);
            await page.GetByLabel("Username or email address").FillAsync(email);                                        await Task.Delay(1000);
            await page.GetByLabel("Username or email address").PressAsync("Tab");                                       await Task.Delay(1000);
            await page.GetByLabel("Password").FillAsync(password);                                                      await Task.Delay(1000);
            await page.GetByLabel("Password").PressAsync("Enter");                                                      await Task.Delay(4000);
            Assert.IsFalse(await page.Locator("text='Sign in'").IsVisibleAsync());
            Assert.IsTrue(await page.Locator("text='Sign out'").IsVisibleAsync());
            Assert.IsTrue(await page.Locator("text='About me'").IsVisibleAsync());
            Assert.IsTrue(await page.Locator("text='User timeline'").IsVisibleAsync());
            Assert.IsTrue(await page.Locator("textarea").IsVisibleAsync(), "Textarea should be visible on the page");

            // Seed new test data with other user following another different user 
            //      User (B) follows User (C) and (D), User (C) follows User (D) 
            // await page.GetByPlaceholder("Write your cheep here!").ClickAsync();                                         await Task.Delay(2000);
            await page.Locator("textarea").ClickAsync();                                                                await Task.Delay(2000);
            await page.Locator("textarea").FillAsync("I will reset and seed new test data");     await Task.Delay(2000);
            await page.GetByRole(AriaRole.Link, new() { Name = "Seed DB" }).ClickAsync();                               await Task.Delay(2000);
            await page.GetByRole(AriaRole.Button, new() { Name = "Seed DB2" }).ClickAsync();                            await Task.Delay(2000);

            await page.GetByPlaceholder("Write your cheep here!").ClickAsync();                                         await Task.Delay(2000);
            await page.GetByPlaceholder("Write your cheep here!").FillAsync("🎉 Playwright test 🎉");                  await Task.Delay(2000);
            await page.GetByRole(AriaRole.Button, new() { Name = "Chirp!" }).ClickAsync();                              await Task.Delay(2000);

            // I follow two different users (B, C)
            await DisplayMessage(page, "I will follow Happy");
            await page.Locator("li").Filter(new() { HasText = "Happy" }).GetByRole(AriaRole.Button).First.ClickAsync();  
            await DisplayMessage(page, "I will follow Chirp27");
            await ScrollDown(page);
            await page.Locator("li").Filter(new() { HasText = "Chirp27" }).GetByRole(AriaRole.Button).First.ClickAsync();   

            // Go into my UserTimeline - both of these users cheeps should appear in my user-timeline
            await DisplayMessage(page, "Navigate to: User timeline");
            await page.GetByText("User Timeline").ClickAsync(); 
            await DisplayMessage(page, "I should see all my followers cheeps + my cheeps");
            await ScrollDown(page);
            Assert.IsTrue(await page.Locator("text='Chirp27'").CountAsync() > 1, "User not found");
            Assert.IsTrue(await page.Locator("text='Happy'").CountAsync() > 1, "User not found");
            Assert.IsFalse(await page.Locator("text='Smiley'").IsVisibleAsync());

            // I unfollow one of the user (C) - his cheeps should not appear in my timeline anymore
            await DisplayMessage(page, "I will unfollow Happy");
            await page.Locator("li:has-text('Happy')").First.GetByRole(AriaRole.Button).GetByText("Unfollow").ClickAsync(); 
            await DisplayMessage(page, "Happy's cheeps are now gone from my timeline");
            Assert.IsFalse(await page.Locator("text='Happy'").IsVisibleAsync());

            // Go to my timeline of user (B) - his timelime should not display his followers cheeps
            await DisplayMessage(page, "Navigate to: Chirp27's timeline");
            // await page.Locator("li:has-text('Chirp27')").First.Locator("#user").ClickAsync(); 
            await page.Locator("#followers").GetByRole(AriaRole.Link, new() { Name = "Chirp27" }).ClickAsync();
            await DisplayMessage(page, "I should only be able to see Chirp27's cheeps");
            await ScrollDown(page);
            // Assert.IsFalse(await page.Locator("text='Happy'").IsVisibleAsync());
            // Assert.IsFalse(await page.Locator("text='Smiley'").IsVisibleAsync());
            Assert.IsFalse(await page.Locator("text='Happy'").CountAsync() > 1, "User not found");
            Assert.IsFalse(await page.Locator("text='Smiley'").CountAsync() > 1, "User not found");

            // Unfollowing User (B) (B's cheeps should not appear in my timeline anymore)
            await DisplayMessage(page, "I will unfollow Chirp27");
            await ScrollDown(page);
            await page.GetByRole(AriaRole.Button, new() { Name = "Unfollow" }).First.ClickAsync(); 

            // Go to my timeline
            await DisplayMessage(page, "Navigate to: User timeline");
            await page.GetByText("User Timeline").ClickAsync(); await Task.Delay(2000);
            await DisplayMessage(page, "My timeline should only consists of my own cheeps now");
            await ScrollDown(page);
            Assert.IsFalse(await page.Locator("text='Chirp27'").CountAsync() > 1, "User's Cheep not found");
            Assert.IsFalse(await page.Locator("text='Happy'").CountAsync() > 1, "User's Cheep not found");
            Assert.IsFalse(await page.Locator("text='Follow'").IsVisibleAsync());
            Assert.IsFalse(await page.Locator("text='Unfollow'").IsVisibleAsync());

            // Go to About me
            await DisplayMessage(page, "Navigate to: About me, and delete my account");
            await page.GetByRole(AriaRole.Link, new() { Name = "About me" }).ClickAsync();                  await Task.Delay(3000);

            // // Forget me
            await page.GetByRole(AriaRole.Button, new() { Name = "Forget Me" }).ClickAsync();               await Task.Delay(5000);
        }
    }
}