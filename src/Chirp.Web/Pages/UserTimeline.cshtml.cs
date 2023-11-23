using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FluentValidation.Results;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private IValidator<CheepDto> _validator;

    private readonly ICheepRepository _cheepRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFollowerRepository _followerRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<UserDto> Followers { get; set; } = new List<UserDto>();
    public int CurrentPage { get; set; } = 1;
    public int MaxCheepsPerPage { get; set; } = 32;
    public int TotalPageCount { get; set; }
    public int StartPage { get; set; }
    public int EndPage { get; set; }
    public int DisplayRange { get; set; } = 5;
    public string? RouteName { get; set; }
    public int CheepMaxLength { get; set; } = 160;
    public Dictionary<string, bool> FollowStatus { get; set; } = new Dictionary<string, bool>();


    [BindProperty, StringLength(160), Required]
    public string? CheepMessage { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IUserRepository userRepository, IFollowerRepository followerRepository, IValidator<CheepDto> validator)
    {
        _cheepRepository = cheepRepository;
        _userRepository = userRepository;
        _followerRepository = followerRepository;
        _validator = validator;
    }

    public ActionResult OnGet(string user)
    {
        //The following if statement has been made with the help of CHAT-GPT
        if (Request.Query.TryGetValue("page", out var pageValues) && int.TryParse(pageValues, out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }
        
        //Creates timeline from original cheeps and followees cheeps
        Followers = _followerRepository.GetFolloweesFromUser(user);
        Cheeps = _cheepRepository.GetCheepsFromUser(user);

        // Set follow status for each cheep user
        if (User.Identity?.IsAuthenticated == true)
        {
            // Include followers' cheeps only for the logged-in user's timeline
            if (user == User.Identity.Name)
            {
                foreach (var userDto in Followers)
                {
                    Cheeps = Cheeps.Union(_cheepRepository.GetCheepsFromUser(userDto.Name));
                }
            }   
            foreach (var cheep in Cheeps)
            {
                var userName = cheep.UserName;
                var isFollowing = _followerRepository
                    .GetFollowersFromUser(userName)
                    .Any(follower => follower.Name == User.Identity.Name);

                FollowStatus[userName] = isFollowing;
            }
        }
        
        Cheeps = Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Skip((CurrentPage - 1) * MaxCheepsPerPage)
            .Take(MaxCheepsPerPage);
        
        // ----------------------------------------------------------

        if (GetTotalPages(user) == 0)
        {
            TotalPageCount = 1;
        } else {
            TotalPageCount = GetTotalPages(user);
        }
        CalculatePagination();

        // User's name
        RouteName = HttpContext.GetRouteValue("user")?.ToString() ?? "";

        return Page();
    }

    public int GetTotalPages(string user)
    {
        int totalCheeps = _cheepRepository.GetCheepsFromUser(user).Count();
        return (int)Math.Ceiling((double)totalCheeps / MaxCheepsPerPage);
    }

    private void CalculatePagination()
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
    
    public IActionResult OnPostFollow(string userName, string followerName)
    {
        if (userName is null)
        {
            throw new ArgumentNullException($"Username is null {nameof(userName)}");
        }
        if (followerName is null)
        {
            throw new ArgumentNullException($"Followername is null {nameof(followerName)}");
        }
        
        _followerRepository.AddOrRemoveFollower(userName, followerName);

        return RedirectToPage(""); //TODO Needs to be changes so it does not redirect but instead refreshes at the same point
    }

    public ActionResult OnPostChirp()
    {
        try
        {
            if (User.Identity?.Name != null)
            {
                var user = new UserDto
                {
                    Name = User.Identity.Name, //Might need to be changed to use only User.Identity (Does not work until users are implemented)
                    Email = User.Identity.Name + "@chirp.com" //TODO: Needs to be removed
                };

                _userRepository.CreateUser(user);
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }

        // TODO Refactor to a class called Utility
        // Added one hour to UTC time to match the time of Copenhagen
        DateTime currentUtcTime = DateTime.UtcNow.AddHours(1);


        var cheep = new CheepDto
        {
            Message = CheepMessage?.Replace("\r\n", " ") ?? "",
            TimeStamp = currentUtcTime.ToString(),
            UserName = User.Identity?.Name ?? "Anonymous"
        };
        
        ValidationResult result = _validator.Validate(cheep);

        if (result.IsValid) _cheepRepository.CreateCheep(cheep);

        
        return RedirectToPage("Public");
    }    
}
