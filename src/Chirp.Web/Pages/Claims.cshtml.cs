using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class ClaimsModel : PageModel
{
    public string? NameID { get; set; }
    public string? Username { get; set; }
    public string? Name { get; set; }
    public string? GithubURL { get; set; }

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            return RedirectToPage("/Public");
        }        
        else
        {
            // nameidentifier, name, github:name, github:url
            string[] claims = new string[4];

            int i = 0;
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                claims[i] = claim.Value;
                i++;
            }

            NameID = claims[0];
            Username = claims[1];
            Name = claims[2];
            GithubURL = claims[3];
        }
        return Page();
    }
}

