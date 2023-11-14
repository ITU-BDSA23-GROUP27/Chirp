using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class ClaimsModel : PageModel
{
    public string? GithubURL { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            return RedirectToPage("/Public");
        }        
        else
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            GithubURL = User.FindFirst("urn:github:url")?.Value;
        }
        return Page();
    }
    
    public IActionResult OnPostAuthenticateLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/"),
        };
        return Challenge(props);
    }

    public IActionResult OnPostLogOut()
    {
        HttpContext.SignOutAsync();
        return RedirectToPage("Public");
    }
}