using System.Globalization;
using System.Text;
using System.Web.Http;
using Chirp.Core;
using Chirp.Core.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

/// <summary>
/// PageModel for the AboutMe page that shows the information about User including claims, followers, followees and Cheeps.
/// The page allows Users to download their data and delete their account.
/// The AboutMe page is only accessible for authenticated users.
/// </summary>

[Authorize]
public class AboutMeModel : BasePageModel
{
    // Cheeps and Followers
    private readonly ICheepRepository _cheepRepository;
    private readonly IFollowerRepository _followerRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReactionRepository _reactionRepository;
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

    public AboutMeModel(ICheepRepository cheepRepository, IFollowerRepository followerRepository, IUserRepository userRepository, IReactionRepository reactionRepository)
    {
        _cheepRepository = cheepRepository;
        _followerRepository = followerRepository;
        _userRepository = userRepository;
        _reactionRepository = reactionRepository;
    }

    public async Task<IActionResult> OnGet()
    {
        if (!await HandleUserAuthenticationAndClaims())
        {
            return Unauthorized();
        }

        if (User.Identity?.IsAuthenticated == true)
        {
            try
            {
                var newUser = new UserDto
                {
                    Name = User.Identity?.Name ?? "Unknown", // Use null conditional operator with null coalescing operator
                    Email = (User.Identity?.Name ?? "Unknown") + "@chirp.com" // Use null conditional operator with null coalescing operator
                };

                await _userRepository.CreateUser(newUser);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Fetch cheeps and followers
        if (Username != null)
        {
            //! ChatGPT's parser -  Parse the page parameter
            var pageValues = Request.Query["page"].ToString();
            if (int.TryParse(pageValues, out int parsedPage) && parsedPage > 0)
            {
                CurrentPage = parsedPage;
            }

            Cheeps = await _cheepRepository.GetCheepsFromUser(Username);
            Followers = (await _followerRepository.GetFolloweesFromUser(Username)).OrderBy(u => u.Name).ToList();
            Followees = (await _followerRepository.GetFollowersFromUser(Username)).OrderBy(u => u.Name).ToList();

            // Pagination
            TotalPageCount = await GetTotalPages(Username) == 0 ? 1 : await GetTotalPages(Username);
        }
        await CalculatePagination();

        Cheeps = Cheeps
            .OrderByDescending(c => DateTime.ParseExact(c.TimeStamp, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture))
            .Skip((CurrentPage - 1) * MaxCheepsPerPage)
            .Take(MaxCheepsPerPage);

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

        if (EndPage - StartPage + 1 >= DisplayRange)
        {
            return Task.CompletedTask;
        }

        if (StartPage > 1)
        {
            StartPage = Math.Max(1, EndPage - DisplayRange + 1);
        }
        else if (EndPage < TotalPageCount)
        {
            EndPage = Math.Min(TotalPageCount, StartPage + DisplayRange - 1);
        }

        return Task.CompletedTask;
    }

    public async Task<IActionResult> OnPostDownloadData()
    {
        if (!await HandleUserAuthenticationAndClaims())
        {
            return Unauthorized();
        }

        // Fetch cheeps and followers
        if (Username != null)
        {
            Cheeps = await _cheepRepository.GetCheepsFromUser(Username);
            Followers = (await _followerRepository.GetFolloweesFromUser(Username)).OrderBy(u => u.Name).ToList();
            Followees = (await _followerRepository.GetFollowersFromUser(Username)).OrderBy(u => u.Name).ToList();
        }

        // Create a string containing user data
        var userData = $"Your user data in Chirp27! \nRetrieved at {DateTime.Now}\n\n";
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
        userData += new string('_', 75) + "\n\nCheeps:\n";
        foreach (var cheep in Cheeps)
        {
            userData += $"\t- Username: {cheep.UserName}\n";
            userData += $"\t- Timestamp: {cheep.TimeStamp}\n";
            userData += $"\t- Message: {cheep.Message}\n\n";
        }

        //! ChatGPT
        // Convert user data to bytes
        var fileContent = Encoding.UTF8.GetBytes(userData);

        // Return a FileContentResult with the content and content type
        return new FileContentResult(fileContent, "text/plain")
        {
            FileDownloadName = "Chirp27 - My User Data.txt"
        };
    }
    
    public async Task<IActionResult> OnPostForgetMe()
    {
        if (!await HandleUserAuthenticationAndClaims())
        {
            return Unauthorized();
        }
        
        if (Username is not null)
        {
            Console.WriteLine("Deleting user: " + Username);

            var user = await _userRepository.GetUserByName(Username);
            
            await _userRepository.DeleteUser(user);
        }
        
        await HttpContext.SignOutAsync();
        
        return RedirectToPage("Public");
    }

    private async Task<bool> HandleUserAuthenticationAndClaims()
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            await HandleNotAuthenticated();
            return false;
        }

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

        return true;
    }
    
    public async Task<IActionResult> OnPostLikeCheep(Guid cheepId, string userName)
    {
        return await HandleLike(cheepId, userName, _reactionRepository);
    }
    public async Task<int> GetLikeCount(Guid cheepId)
    {
        return await HandleGetLikeCount(cheepId, _reactionRepository);
    }

    public async Task<bool> HasUserLikedCheep(Guid cheepId, string userName)
    {
        return await HandleHasUserLikedCheep(cheepId, userName, _reactionRepository);
    }

    public async Task<IActionResult> OnPostLogOut()
    {
        return await HandleLogOut();
    }
}