using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class ClaimsModel : PageModel
{
    public string? NameID { get; set; }
    public string? DisplayName { get; set; }
    public string? GithubName { get; set; }
    public string? GithubURL { get; set; }

    public void  OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
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
            DisplayName = claims[1];
            GithubName = claims[2];
            GithubURL = claims[3];
        }
    }
}

