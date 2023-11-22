using Chirp.Core;
using Chirp.Core.DTOs;
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
            TimeStamp = currentUtcTime.ToString(),
            AuthorName = User.Identity?.Name ?? "Anonymous"
        };

        ValidationResult result = validator.Validate(cheep);

        if (result.IsValid) cheepRepository.CreateCheep(cheep);
        
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
}