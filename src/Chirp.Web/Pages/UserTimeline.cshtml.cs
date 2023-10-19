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

    public int TotalPageCount { get; set; }
    public int StartPage { get; set; }
    public int EndPage { get; set; }
    public int DisplayRange { get; set; } = 5;

    public string RouteName { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository)
    {
        _cheepRepository = cheepRepository;
    }

    public ActionResult OnGet(string author)
    {
        //The following if statement has been made with the help of CHAT-GPT
        if (Request.Query.TryGetValue("page", out var pageValues) && int.TryParse(pageValues, out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }

        Cheeps = _cheepRepository.GetCheepsFromAuthorPage(author, CurrentPage);

        TotalPageCount = GetTotalPages(author);
        CalculatePagination();

        // Author's name
        RouteName = HttpContext.GetRouteValue("author")?.ToString() ?? "";

        return Page();
    }

    public int GetTotalPages(string author)
    {
        int totalCheeps = _cheepRepository.GetCheepsFromAuthor(author).Count();
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
}
