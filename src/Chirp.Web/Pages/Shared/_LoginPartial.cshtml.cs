using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Shared;

public class LoginPartialModel : PageModel
{
    public void OnGet()
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
    }

    public IActionResult OnPostAuthenticateLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/Error"),
        };
        return Challenge(props);
    }

    public IActionResult OnPostLogOut()
    {
        HttpContext.SignOutAsync();
        return RedirectToPage();
    }
}