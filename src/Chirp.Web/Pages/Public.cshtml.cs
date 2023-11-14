using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Web.Areas.Identity.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public int CurrentPage { get; set; } = 1;
    public int MaxCheepsPerPage { get; } = 32;
    public int TotalPageCount { get; set; }
    public int StartPage { get; set; }
    public int EndPage { get; set; }
    public int DisplayRange { get; set; } = 5;
    public int CheepMaxLength { get; set; } = 160;
    
    
    [BindProperty, StringLength(160), Required]
    public string? CheepMessage { get; set; }

    public PublicModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public ActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            Console.WriteLine(User.Identity.Name);
        }
        else
        {
            Console.WriteLine("Not Authenticated");
        }
        
        //The following if statement has been made with the help of CHAT-GPT
        if (int.TryParse(Request.Query["page"], out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }

        Cheeps = _cheepRepository.GetCheepsFromPage(CurrentPage);

        if (GetTotalPages() == 0)
        {
            TotalPageCount = 1;    
        } else {
            TotalPageCount = GetTotalPages();
        }
        CalculatePagination();

        return Page();
    }
    public ActionResult OnPostChirp()
    {
        if (CheepMessage.Length > 160) //TODO Enters accounts for 2 characters
        {
            throw new ArgumentException($"Message cannot be longer than 160 characters. Message was: {CheepMessage.Length} characters long");
        }

        try
        {
            var author = new AuthorDto
            {
                Name = User.Identity.Name, //Might need to be changed to use only User.Identity (Does not work until users are implemented)
                Email = User.Identity.Name + "@chirp.com" //TODO: Needs to be removed
            };

            _authorRepository.CreateAuthor(author);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        var cheep = new CheepDto
        {
            Message = CheepMessage,
            TimeStamp = DateTime.Now.ToString(),
            AuthorName = User.Identity.Name //Might need to be changed to use only User.Identity (Does not work until users are implemented)
        };
        
        _cheepRepository.CreateCheep(cheep);
        
        return RedirectToPage("Public");
    }

    public IActionResult OnPostAuthenticateLogin()
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/"),
        };
        return Challenge(props);
    }

    public IActionResult OnPostLogOut()
    {
        HttpContext.SignOutAsync();
        return RedirectToPage("Public");
    }
    
    public int GetTotalPages()
    {
        int totalCheeps = _cheepRepository.GetCheeps().Count();
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
