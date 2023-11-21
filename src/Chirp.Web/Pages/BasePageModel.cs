using Chirp.Core;
using Chirp.Core.DTOs;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class BasePageModel : PageModel
{
    protected IActionResult HandleNotAuthenticated()
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            return RedirectToPage("/Public");
        }

        return Page();
    }
    
    protected IActionResult Chirp(string? cheepMessage, IValidator<CheepDto> validator, ICheepRepository cheepRepository)
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
        
        return RedirectToPage("Public");
    }
}