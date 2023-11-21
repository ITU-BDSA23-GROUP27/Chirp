using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Chirp.Web.Areas.Identity.Pages;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FluentValidation.Results;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Chirp.Web.Pages;

public class UserTimelineModel : BasePageModel
{
    private IValidator<CheepDto> _validator;

    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IFollowerRepository _followerRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<AuthorDto> Followers { get; set; } = new List<AuthorDto>();
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

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IFollowerRepository followerRepository, IValidator<CheepDto> validator)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        _followerRepository = followerRepository;
        _validator = validator;
    }

    public ActionResult OnGet(string author)
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            try
            {
                
                //TODO Problem: Can't find github authors in http since it makes it lowercase e.g. "/Tien197" -> "/tien197"   
                var existingAuthor = _authorRepository.GetAuthorByName(author);
                
                // show cheeps of author
            }
            catch (ArgumentException e)
            {
                return RedirectToPage("/Public");
            }
            
        }
        
        //The following if statement has been made with the help of CHAT-GPT
        if (Request.Query.TryGetValue("page", out var pageValues) && int.TryParse(pageValues, out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }
        
        //Creates timeline from original cheeps and followees cheeps
        Followers = _followerRepository.GetFolloweesFromAuthor(author);
        Cheeps = _cheepRepository.GetCheepsFromAuthor(author);

        // Set follow status for each cheep author
        if (User.Identity?.IsAuthenticated == true)
        {
            // Include followers' cheeps only for the logged-in user's timeline
            if (author == User.Identity.Name)
            {
                foreach (var authorDto in Followers)
                {
                    Cheeps = Cheeps.Union(_cheepRepository.GetCheepsFromAuthor(authorDto.Name));
                }
            }   
            foreach (var cheep in Cheeps)
            {
                var authorName = cheep.AuthorName;
                var isFollowing = _followerRepository
                    .GetFollowersFromAuthor(authorName)
                    .Any(follower => follower.Name == User.Identity.Name);

                FollowStatus[authorName] = isFollowing;
            }
        }
        
        Cheeps = Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Skip((CurrentPage - 1) * MaxCheepsPerPage)
            .Take(MaxCheepsPerPage);
        
        // ----------------------------------------------------------

        if (GetTotalPages(author) == 0)
        {
            TotalPageCount = 1;
        } else {
            TotalPageCount = GetTotalPages(author);
        }
        CalculatePagination();

        // Author's name
        RouteName = HttpContext.GetRouteValue("author")?.ToString() ?? "";

        return Page();
    }

    public int GetTotalPages(string author)
    {
        int totalCheeps = _cheepRepository.GetCheepsFromAuthor(author).Count();
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
        return HandleAuthenticateLogin();
    }

    public IActionResult OnPostLogOut()
    {
        return HandleLogOut();
    }
    
    public IActionResult OnPostFollow(string authorName, string followerName)
    {
        return HandleFollow(authorName, followerName, _followerRepository);
    }
    
    public IActionResult OnPostChirp()
    {
        return Chirp(CheepMessage, _validator, _cheepRepository);
    }    
}
