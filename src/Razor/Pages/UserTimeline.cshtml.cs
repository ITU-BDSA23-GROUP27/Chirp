using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }
    public int CurrentPage { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author)
    {
        //The following if statement has been made with the help of CHAT-GPT
        if (int.TryParse(Request.Query["page"], out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }
        Cheeps = _service.GetCheepsFromAuthor(author, CurrentPage);
        return Page();
    }
}
