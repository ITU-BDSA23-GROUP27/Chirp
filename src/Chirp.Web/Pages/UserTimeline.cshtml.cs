using Chirp.Core;
using Chirp.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int MaxCheepsPerPage { get; } = 32;

    public UserTimelineModel(ICheepRepository cheepRepository)
    {
        _cheepRepository = cheepRepository;
    }

    public ActionResult OnGet(string author)
    {
        //The following if statement has been made with the help of CHAT-GPT
        if (int.TryParse(Request.Query["page"], out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }
        Cheeps = _cheepRepository.GetCheepsFromAuthor(author, CurrentPage);
        return Page();
    }

    public int GetTotalPages(string author)
    {
        int totalCheeps = _cheepRepository.GetCheepsFromAuthor(author, 1).Count();
        return (int)Math.Ceiling((double)totalCheeps / MaxCheepsPerPage);
    }
}
