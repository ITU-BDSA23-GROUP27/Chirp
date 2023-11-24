using System.Text;
using Chirp.Core;
using Chirp.Core.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class AboutMeModel : BasePageModel
{
    // Cheeps and Followers
    private readonly ICheepRepository _cheepRepository;
    private readonly IFollowerRepository _followerRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<UserDto> Followers { get; set; } = new List<UserDto>();
    public IEnumerable<UserDto> Followees { get; set; } = new List<UserDto>();

    // Pagination
    public int CurrentPage { get; set; } = 1;
    public int MaxCheepsPerPage { get; set; } = 32;
    public int TotalPageCount { get; set; }
    public int StartPage { get; set; }
    public int EndPage { get; set; }
    public int DisplayRange { get; set; } = 5;

    // User Claims
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

        // Pagination
        if (Username != null)
        {
            TotalPageCount = await GetTotalPages(Username) == 0 ? 1 : await GetTotalPages(Username);
        }
        await CalculatePagination();

        return await Task.FromResult<IActionResult>(Page());
    }

    public async Task<int> GetTotalPages(string user)
    {
        int totalCheeps = (await _cheepRepository.GetCheepsFromUser(user)).Count();
        return (int)Math.Ceiling((double)totalCheeps / MaxCheepsPerPage);
    }

    public Task CalculatePagination()
    {
        StartPage = Math.Max(1, CurrentPage - DisplayRange / 2);
        EndPage = Math.Min(TotalPageCount, StartPage + DisplayRange - 1);

        if (EndPage - StartPage + 1 < DisplayRange)
        {
            if (StartPage > 1)
            {
                StartPage = Math.Max(1, EndPage - DisplayRange + 1);
            }
            else if (EndPage < TotalPageCount)
            {
                EndPage = Math.Min(TotalPageCount, StartPage + DisplayRange - 1);
            }
        }

        return Task.CompletedTask;
    }


    public async Task<IActionResult> OnPostDownloadData()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return Unauthorized();
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

        // Create a string containing user data
        var userData = $"Your user data in Chirp! GR27 \nRetrieved at {DateTime.Now}\n\n";
        userData += new string('_', 75) + $"\n\nClaims: \n\t- ID: {ID} \n\t- Username: {Username} \n\t- Email: {Email} \n\t- Name: {Name} \n\t- GitHub URL: {GithubURL} \n\t- Avatar: {Avatar}\n\n";

        // TODO - reverse names followers and followees?
        // Add followers to the string
        userData += new string('_', 75) + "\n\nFollowing:\n";
        foreach (var follower in Followers)
        {
            userData += $"\t - {follower.Name}\n";
        }
        userData += "\n";

        // Add followees to the string
        userData += new string('_', 75) + "\n\nFollowees:\n";
        foreach (var followee in Followees)
        {
            userData += $"\t - {followee.Name}\n";
        }
        userData += "\n";

        // Add cheeps to the string
        userData += new string('_', 75) + "\n\nMy Cheeps:\n";
        foreach (var cheep in Cheeps)
        {
            userData += $"\t- Username: {cheep.UserName}\n";
            userData += $"\t- Timestamp: {cheep.TimeStamp}\n";
            userData += $"\t- Message: {cheep.Message}\n\n";
        }
        userData += "\n";

        // empty
        return await Task.FromResult<IActionResult>(Page());
    }

    public async Task<IActionResult> OnPostLogOut()
    {
        return await HandleLogOut();
    }
}