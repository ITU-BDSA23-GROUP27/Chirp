using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public IQueryable<CheepViewModel> Cheeps { get; set; }

    public int CurrentPage { get; set; } = 1;
    //public int MaxCheepsPerPage { get; } = 32;

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        //The following if statement has been made with the help of CHAT-GPT
        if (int.TryParse(Request.Query["page"], out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }
        
        Cheeps = _service.GetCheeps(CurrentPage);
        return Page();
    }
}
