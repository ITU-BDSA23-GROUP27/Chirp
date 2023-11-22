using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class ProfileModel : BasePageModel
{
    public string? GithubURL { get; set; }

    public async Task<IActionResult> OnGet()
    {
        if (User.Identity?.IsAuthenticated == false) return await HandleNotAuthenticated();
               
        foreach (var claim in User.Claims)
        {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }

        GithubURL = User.FindFirst("urn:github:url")?.Value;
        
        return await Task.FromResult<IActionResult>(Page());
    }
    
    public async Task<IActionResult> OnPostLogOut()
    {
        return await HandleLogOut();
    }
}