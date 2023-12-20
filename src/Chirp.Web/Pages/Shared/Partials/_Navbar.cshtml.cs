using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Shared.Partials;

public class NavigationPartialModel : PageModel
{
    public Task OnGet()
    {
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                Console.WriteLine(User.Identity.Name);
            }
            else
            {
                Console.WriteLine("Not Authenticated");
            }
        }
        return Task.CompletedTask;
    }

    public async Task<IActionResult> OnPostAuthenticateLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/Error")
        };
        return await Task.FromResult<IActionResult>(Challenge(props));
    }

    public async Task<IActionResult> OnPostLogOut()
    {
        await HttpContext.SignOutAsync();
        return RedirectToPage();
    }
}