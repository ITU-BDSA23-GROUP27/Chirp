using Chirp.Core;
using Chirp.Core.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class AboutMeModel : BasePageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IFollowerRepository _followerRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<UserDto> Followers { get; set; } = new List<UserDto>();
    public IEnumerable<UserDto> Followees { get; set; } = new List<UserDto>();

    public string? ID { get; set; }
    public string? Username { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? GithubURL { get; set; }
    public string? Avatar { get; set; }

    public AboutMeModel(ICheepRepository cheepRepository, IFollowerRepository followerRepository)
    {
        _cheepRepository = cheepRepository;
        _followerRepository = followerRepository;
    }

    public async Task<IActionResult> OnGet()
    {
        if (User.Identity?.IsAuthenticated == false) 
            return await HandleNotAuthenticated();
               
        // User Claims
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

        // Fetch cheeps and followers
        if (Username != null)
        {
            Cheeps = await _cheepRepository.GetCheepsFromUser(Username);
            Followers = (await _followerRepository.GetFolloweesFromUser(Username)).OrderBy(u => u.Name).ToList();
            Followees = (await _followerRepository.GetFollowersFromUser(Username)).OrderBy(u => u.Name).ToList();
        }

        return await Task.FromResult<IActionResult>(Page());
    }
    
    public async Task<IActionResult> OnPostLogOut()
    {
        return await HandleLogOut();
    }
}