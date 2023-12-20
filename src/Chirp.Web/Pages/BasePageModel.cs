using System.Globalization;
using Chirp.Core;
using Chirp.Core.DTOs;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

/// <summary>
/// A base class for all PageModels in the application.
/// The base class contains methods that are used in multiple PageModels to avoid code duplication.
/// </summary>

public class BasePageModel : PageModel
{
    protected async Task<IActionResult> HandleNotAuthenticated()
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            return await Task.FromResult<IActionResult>(RedirectToPage("/Public"));
        }

        return await Task.FromResult<IActionResult>(Page());
    }

    public async Task<IActionResult> Cheep(string? cheepMessage, IValidator<CheepDto> validator, ICheepRepository cheepRepository)
    {
        // TODO Refactor to a class called Utility
        // Added one hour to UTC time to match the time of Copenhagen
        DateTime currentUtcTime = DateTime.UtcNow.AddHours(1);
        
        var cheep = new CheepDto
        {
            Message = cheepMessage?.Replace("\r\n", " ") ?? "",
            TimeStamp = currentUtcTime.ToString(),
            UserName = User.Identity?.Name ?? "Anonymous"
        };

        ValidationResult result = await validator.ValidateAsync(cheep);

        if (result.IsValid) await cheepRepository.CreateCheep(cheep);
        
        return await Task.FromResult<IActionResult>(RedirectToPage("Public"));
    }

    protected async Task<IActionResult> HandleAuthenticateLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/"),
        };
        return await Task.FromResult<IActionResult>(Challenge(props));
    }
    
    protected async Task<IActionResult> HandleLogOut()
    {
        await HttpContext.SignOutAsync();
        return await Task.FromResult<IActionResult>(RedirectToPage("Public"));
    }

    public async Task<IActionResult> HandleFollow(string authorName, string followerName, IFollowerRepository followerRepository)
    {
        if (authorName is null)
        {
            throw new ArgumentNullException($"Authorname is null {nameof(authorName)}");
        }
        if (followerName is null)
        {
            throw new ArgumentNullException($"Followername is null {nameof(followerName)}");
        }
        
        await followerRepository.AddOrRemoveFollower(authorName, followerName);

        return await Task.FromResult<IActionResult>(RedirectToPage(""));
    }

    public async Task<IActionResult> HandleLike(Guid cheepId, string userName, IReactionRepository reactionRepository)
    {
        if (userName is null)
        {
            throw new ArgumentNullException($"Username is null");
        }
        
        await reactionRepository.LikeCheep(cheepId, userName);
        
        return await Task.FromResult<IActionResult>(RedirectToPage(""));
    }

    public async Task<int> HandleGetLikeCount(Guid cheepId, IReactionRepository reactionRepository)
    {
        return await reactionRepository.GetLikeCount(cheepId);
    }

    public async Task<bool> HandleHasUserLikedCheep(Guid cheepId, string userName, IReactionRepository reactionRepository)
    {
        return await reactionRepository.HasUserLiked(cheepId, userName);
    }
    
    protected async Task<IActionResult> HandleComment(string? comment, Guid userId, Guid cheepId, IValidator<ReactionDto> validator, IReactionRepository reactionRepository)
    {

        var reactionDto = new ReactionDto()
        {
            UserId = userId,
            CheepId = cheepId,
            TimeStamp = DateTime.UtcNow.AddHours(1).ToString(CultureInfo.InvariantCulture),
            Comment = comment?.Replace("\r\n", " ") ?? ""
        };
        
        ValidationResult result = await validator.ValidateAsync(reactionDto);

        if (result.IsValid) await reactionRepository.CommentOnCheep(reactionDto);
        return await Task.FromResult<IActionResult>(RedirectToPage(""));
    }
}