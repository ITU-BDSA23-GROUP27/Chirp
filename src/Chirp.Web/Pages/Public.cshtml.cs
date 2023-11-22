using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Core.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class PublicModel : BasePageModel
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

    public async Task<ActionResult> OnGet()
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

                await _authorRepository.CreateAuthor(author);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        Cheeps = await _cheepRepository.GetCheepsFromPage(CurrentPage);
        
        // Set follow status for each cheep author
        if (User.Identity?.IsAuthenticated == true)
        {
            foreach (var cheep in Cheeps)
            {
                var authorName = cheep.AuthorName;
                var followersFromAuthor = await _followerRepository.GetFollowersFromAuthor(authorName);
                var isFollowing = followersFromAuthor.Any(follower => follower.Name == User.Identity.Name);

                FollowStatus[authorName] = isFollowing;
            }
        }

        //The following if statement has been made with the help of CHAT-GPT
        if (int.TryParse(Request.Query["page"], out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }

        Cheeps = await _cheepRepository.GetCheepsFromPage(CurrentPage);

        if (await GetTotalPages() == 0)
        {
            TotalPageCount = 1;    
        } else {
            TotalPageCount = await GetTotalPages();
        }
        CalculatePagination();

        return Page();
    }
    
    public async Task<int> GetTotalPages()
    {
        int totalCheeps = (await _cheepRepository.GetCheeps()).Count();
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