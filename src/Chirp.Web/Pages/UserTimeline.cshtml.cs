﻿using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Chirp.Core;
using Chirp.Core.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

/// <summary>
/// PageModel for the UserTimeline page that shows the timeline for a specific user.
/// Users can create Cheeps and react to their own Cheeps from the UserTimeline page.
/// The page also shows the followers and followees of the User.
/// Both authenticated and unauthenticated users can access the UserTimeline page
/// </summary>

public class UserTimelineModel : BasePageModel
{
    private IValidator<CheepDto> _validator;

    private readonly ICheepRepository _cheepRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFollowerRepository _followerRepository;
    private readonly IReactionRepository _reactionRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public IEnumerable<UserDto> Followers { get; set; } = new List<UserDto>();
    public IEnumerable<UserDto> Followees { get; set; } = new List<UserDto>();
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

    public UserTimelineModel(ICheepRepository cheepRepository, IUserRepository userRepository, 
                             IFollowerRepository followerRepository, IReactionRepository reactionRepository, IValidator<CheepDto> validator)
    {
        _cheepRepository = cheepRepository;
        _userRepository = userRepository;
        _followerRepository = followerRepository;
        _reactionRepository = reactionRepository;
        _validator = validator;
    }

    public async Task<ActionResult> OnGet(string user)
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
        
        try
        {
            var existingUser = await _userRepository.GetUserByName(user); 
        }
        catch (ArgumentException)
        {
            return RedirectToPage("/Error");
        }
        
        //The following if statement has been made with the help of CHAT-GPT
        if (Request.Query.TryGetValue("page", out var pageValues) && int.TryParse(pageValues, out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }
        
        //Creates timeline from original cheeps and followees cheeps
        Followers = (await _followerRepository.GetFolloweesFromUser(user)).OrderBy(u => u.Name).ToList();
        Followees = (await _followerRepository.GetFollowersFromUser(user)).OrderBy(u => u.Name).ToList();
        Cheeps = await _cheepRepository.GetCheepsFromUser(user);

        // Set follow status for each cheep user
        if (User.Identity?.IsAuthenticated == true)
        {
            // Include followers' cheeps only for the logged-in user's timeline
            if (user == User.Identity.Name)
            {
                foreach (var userDto in Followers)
                {
                    var followerCheeps = await _cheepRepository.GetCheepsFromUser(userDto.Name);
                    var cheepDtos = followerCheeps.ToList();
                    Cheeps = Cheeps.Union(cheepDtos);
                    TotalFollowerCheepCount += cheepDtos.Count();
                }
            }
            
            foreach (var cheep in Cheeps)
            {
                var userName = cheep.UserName;
                var followersFromUser = await _followerRepository
                    .GetFollowersFromUser(userName);
                    
                    var isFollowing = followersFromUser.Any(follower => follower.Name == User.Identity.Name);

                FollowStatus[userName] = isFollowing;
            }
        }
        
        Cheeps = Cheeps
            .OrderByDescending(c => DateTime.ParseExact(c.TimeStamp, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture))
            .Skip((CurrentPage - 1) * MaxCheepsPerPage)
            .Take(MaxCheepsPerPage);
        
        // Pagination
        TotalPageCount = await GetTotalPages(user) == 0 ? 1 : await GetTotalPages(user);
        await CalculatePagination();

        // User's name
        RouteName = HttpContext.GetRouteValue("user")?.ToString() ?? "";

        return Page();
    }

    public async Task<int> GetTotalPages(string user)
    {
        int totalCheeps = (await _cheepRepository.GetCheepsFromUser(user)).Count() + TotalFollowerCheepCount;
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
