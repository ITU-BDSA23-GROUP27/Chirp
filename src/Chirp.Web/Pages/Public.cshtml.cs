using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Web.Areas.Identity.Pages;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private IValidator<CheepDto> _validator;
    
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IFollowerRepository _followerRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public int CurrentPage { get; set; } = 1;
    public int MaxCheepsPerPage { get; } = 32;
    public int TotalPageCount { get; set; }
    public int StartPage { get; set; }
    public int EndPage { get; set; }
    public int DisplayRange { get; set; } = 5;
    public int CheepMaxLength { get; set; } = 160;
    public Dictionary<string, bool> FollowStatus { get; set; } = new Dictionary<string, bool>();

    
    [BindProperty, StringLength(160), Required]
    public string? CheepMessage { get; set; }

    public PublicModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IFollowerRepository followerRepository, IValidator<CheepDto> validator)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        _followerRepository = followerRepository;
        _validator = validator;
    }

    public ActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            try
            {
                var author = new AuthorDto
                {
                    Name = User.Identity.Name, //Might need to be changed to use only User.Identity (Does not work until users are implemented)
                    Email = User.Identity.Name + "@chirp.com" //TODO: Needs to be removed
                };

                _authorRepository.CreateAuthor(author);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Not Authenticated");
        }
        
        Cheeps = _cheepRepository.GetCheepsFromPage(CurrentPage);

        // Set follow status for each cheep author
        if (User.Identity?.IsAuthenticated == true)
        {
            foreach (var cheep in Cheeps)
            {
                var authorName = cheep.AuthorName;
                var isFollowing = _followerRepository
                    .GetFollowersFromAuthor(authorName)
                    .Any(follower => follower.Name == User.Identity.Name);

                FollowStatus[authorName] = isFollowing;
            }
        }

        //The following if statement has been made with the help of CHAT-GPT
        if (int.TryParse(Request.Query["page"], out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }

        Cheeps = _cheepRepository.GetCheepsFromPage(CurrentPage);

        if (GetTotalPages() == 0)
        {
            TotalPageCount = 1;    
        } else {
            TotalPageCount = GetTotalPages();
        }
        CalculatePagination();

        return Page();
    }

    public ActionResult OnPostChirp()
    {
        // TODO Refactor to a class called Utility
        // Added one hour to UTC time to match the time of Copenhagen
        DateTime currentUtcTime = DateTime.UtcNow.AddHours(1);
        
        var cheep = new CheepDto
        {
            Message = CheepMessage?.Replace("\r\n", " ") ?? "",
            TimeStamp = currentUtcTime.ToString(),
            AuthorName = User.Identity?.Name ?? "Anonymous"
        };

        ValidationResult result = _validator.Validate(cheep);

        if (result.IsValid) _cheepRepository.CreateCheep(cheep);
        
        return RedirectToPage("Public");
    }

    public IActionResult OnPostFollow(string authorName, string followerName)
    {
        if (authorName is null)
        {
            throw new ArgumentNullException($"Authorname is null {nameof(authorName)}");
        }
        if (followerName is null)
        {
            throw new ArgumentNullException($"Followername is null {nameof(followerName)}");
        }
        
        _followerRepository.AddOrRemoveFollower(authorName, followerName);

        return RedirectToPage(""); //TODO Needs to be changes so it does not redirect but instead refreshes at the same point
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
    
    public int GetTotalPages()
    {
        int totalCheeps = _cheepRepository.GetCheeps().Count();
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
}