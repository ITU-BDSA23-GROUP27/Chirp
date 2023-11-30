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
    private readonly IUserRepository _userRepository;
    private readonly IFollowerRepository _followerRepository;
    private readonly IReactionRepository _reactionRepository;
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

    public PublicModel(ICheepRepository cheepRepository, IUserRepository userRepository,
                       IFollowerRepository followerRepository, IReactionRepository reactionRepository, IValidator<CheepDto> validator)
    {
        _cheepRepository = cheepRepository;
        _userRepository = userRepository;
        _followerRepository = followerRepository;
        _reactionRepository = reactionRepository;
        _validator = validator;
    }

    public async Task<ActionResult> OnGet()
    {
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

        Cheeps = await _cheepRepository.GetCheepsFromPage(CurrentPage);

        // Set follow status for each cheep user
        if (User.Identity?.IsAuthenticated == true)
        {
            foreach (var cheep in Cheeps)
            {
                var userName = cheep.UserName;
                var followersFromUser = await _followerRepository.GetFollowersFromUser(userName);
                var isFollowing = followersFromUser.Any(follower => follower.Name == User.Identity.Name);

                FollowStatus[userName] = isFollowing;
            }
        }

        //The following if statement has been made with the help of CHAT-GPT
        if (int.TryParse(Request.Query["page"], out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }

        Cheeps = await _cheepRepository.GetCheepsFromPage(CurrentPage);

        TotalPageCount = await GetTotalPages() == 0 ? 1 : await GetTotalPages();
        await CalculatePagination();        

        return Page();
    }

    public async Task<int> GetTotalPages()
    {
        int totalCheeps = (await _cheepRepository.GetCheeps()).Count();
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

    public async Task<IActionResult> OnPostCheep()
    {
        return await Cheep(CheepMessage, _validator, _cheepRepository);
    }

    public async Task<IActionResult> OnPostFollow(string userName, string followerName)
    {
        return await HandleFollow(userName, followerName, _followerRepository);

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
    
    public async Task<IActionResult> OnPostAuthenticateLogin()
    {
        return await HandleAuthenticateLogin();
    }

    public async Task<IActionResult> OnPostLogOut()
    {
        return await HandleLogOut();
    }
}