using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class AboutMeModel : BasePageModel
{
    public string? ID { get; set; }
    public string? Username { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? GithubURL { get; set; }
    public string? Avatar { get; set; }

    public async Task<IActionResult> OnGet()
    {
        if (User.Identity?.IsAuthenticated == false) return await HandleNotAuthenticated();
               
        foreach (var claim in User.Claims)
        {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }

        ID = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        Username = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        Email = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
        Name = User.FindFirst("urn:github:name")?.Value;
        GithubURL = User.FindFirst("urn:github:url")?.Value;
        Avatar = $"https://avatars.githubusercontent.com/{Username}";
        
        return await Task.FromResult<IActionResult>(Page());
    }
    
    public async Task<IActionResult> OnPostLogOut()
    {
        return await HandleLogOut();
    }
}