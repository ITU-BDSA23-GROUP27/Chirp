using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class UserTimelineModel : BasePageModel
{
    private IValidator<CheepDto> _validator;

    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IFollowerRepository _followerRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<AuthorDto> Followers { get; set; } = new List<AuthorDto>();
    public IEnumerable<AuthorDto> Followees { get; set; } = new List<AuthorDto>();
    public int CurrentPage { get; set; } = 1;
    public int MaxCheepsPerPage { get; set; } = 32;
    public int TotalPageCount { get; set; }
    public int StartPage { get; set; }
    public int EndPage { get; set; }
    public int DisplayRange { get; set; } = 5;
    public string? RouteName { get; set; }
    public int CheepMaxLength { get; set; } = 160;
    public Dictionary<string, bool> FollowStatus { get; set; } = new Dictionary<string, bool>();
    public int TotalFollowerCheepCount { get; set; }

    [BindProperty, StringLength(160), Required]
    public string? CheepMessage { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IFollowerRepository followerRepository, IValidator<CheepDto> validator)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        _followerRepository = followerRepository;
        _validator = validator;
    }

    public async Task<ActionResult> OnGet(string author)
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            try
            {
                //TODO Problem: Can't find github authors in http since it makes it lowercase e.g. "/Tien197" -> "/tien197"   
                var existingAuthor = await _authorRepository.GetAuthorByName(author);
                
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
        Followers = (await _followerRepository.GetFolloweesFromAuthor(author)).OrderBy(a => a.Name).ToList();
        Followees = (await _followerRepository.GetFollowersFromAuthor(author)).OrderBy(a => a.Name).ToList();
        Cheeps = await _cheepRepository.GetCheepsFromAuthor(author);

        // Set follow status for each cheep author
        if (User.Identity?.IsAuthenticated == true)
        {
            // Include followers' cheeps only for the logged-in user's timeline
            if (author == User.Identity.Name)
            {
                foreach (var authorDto in Followers)
                {
                    var followerCheeps = await _cheepRepository.GetCheepsFromAuthor(authorDto.Name);
                    var cheepDtos = followerCheeps.ToList();
                    Cheeps = Cheeps.Union(cheepDtos);
                    TotalFollowerCheepCount += cheepDtos.Count();
                }
            }   
            foreach (var cheep in Cheeps)
            {
                var authorName = cheep.AuthorName;
                var followersFromAuthor = await _followerRepository
                    .GetFollowersFromAuthor(authorName);
                    
                    var isFollowing = followersFromAuthor.Any(follower => follower.Name == User.Identity.Name);

                FollowStatus[authorName] = isFollowing;
            }
        }
        
        Cheeps = Cheeps
            .OrderByDescending(c => DateTime.ParseExact(c.TimeStamp, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture))
            .Skip((CurrentPage - 1) * MaxCheepsPerPage)
            .Take(MaxCheepsPerPage);
        
        // Pagination
        TotalPageCount = await GetTotalPages(author) == 0 ? 1 : await GetTotalPages(author);
        await CalculatePagination();

        // Author's name
        RouteName = HttpContext.GetRouteValue("author")?.ToString() ?? "";

        return Page();
    }

    public async Task<int> GetTotalPages(string author)
    {
        int totalCheeps = (await _cheepRepository.GetCheepsFromAuthor(author)).Count() + TotalFollowerCheepCount;
        return (int)Math.Ceiling((double)totalCheeps / MaxCheepsPerPage);
    }

    private Task CalculatePagination()
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
    
    public async Task<IActionResult> OnPostChirp()
    {
        return await Chirp(CheepMessage, _validator, _cheepRepository);
    }   
    
    public async Task<IActionResult> OnPostFollow(string authorName, string followerName)
    {
        return await HandleFollow(authorName, followerName, _followerRepository);
    }
    
    public async Task<IActionResult> OnPostAuthenticateLogin()
    {
        return await HandleAuthenticateLogin();
    }

    public async Task<IActionResult> OnPostLogOut()
    {
        return await HandleLogOut();
    }
}
