using System.Globalization;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

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
    
    protected async Task<IActionResult> Chirp(string? cheepMessage, IValidator<CheepDto> validator, ICheepRepository cheepRepository)
    {
        // TODO Refactor to a class called Utility
        // Added one hour to UTC time to match the time of Copenhagen
        DateTime currentUtcTime = DateTime.UtcNow.AddHours(1);
        
        var cheep = new CheepDto
        {
            Message = cheepMessage?.Replace("\r\n", " ") ?? "",
            TimeStamp = currentUtcTime.ToString(CultureInfo.InvariantCulture),
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
    
    protected async Task<IActionResult> HandleFollow(string authorName, string followerName, IFollowerRepository followerRepository)
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

    protected async Task<IActionResult> HandleLike(Guid cheepId, Guid userId, IReactionRepository reactionRepository)
    {
        await reactionRepository.LikeCheep(cheepId, userId);
        
        return await Task.FromResult<IActionResult>(RedirectToPage(""));
    }
    
    protected async Task<IActionResult> HandleComment(string? comment, Guid userId, Guid cheepId, IValidator<CommentDto> validator, IReactionRepository reactionRepository)
    {

        var commentDto = new CommentDto()
        {
            UserId = userId,
            CheepId = cheepId,
            TimeStamp = DateTime.UtcNow.AddHours(1).ToString(CultureInfo.InvariantCulture),
            Comment = comment?.Replace("\r\n", " ") ?? ""
        };
        
        ValidationResult result = await validator.ValidateAsync(commentDto);

        if (result.IsValid) await reactionRepository.CommentOnCheep(commentDto);
        return await Task.FromResult<IActionResult>(RedirectToPage(""));
    }
}