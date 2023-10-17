using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int MaxCheepsPerPage { get; } = 32;

    public PublicModel(ICheepRepository cheepRepository)
    {
        _cheepRepository = cheepRepository;
    }

    public ActionResult OnGet()
    {
        //The following if statement has been made with the help of CHAT-GPT
        if (int.TryParse(Request.Query["page"], out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }

        Cheeps = _cheepRepository.GetCheepsFromPage(CurrentPage);
        return Page();
    }

    public int GetTotalPages()
    {
        int totalCheeps = _cheepRepository.GetCheeps().Count();
        return (int)Math.Ceiling((double)totalCheeps / MaxCheepsPerPage);
    }
}
