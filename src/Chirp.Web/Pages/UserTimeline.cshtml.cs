using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Core.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FluentValidation.Results;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private IValidator<CheepDto> _validator;

    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;    
    public IEnumerable<CheepDto> Cheeps { get; set; } = new List<CheepDto>();
    public int CurrentPage { get; set; } = 1;
    public int MaxCheepsPerPage { get; set; } = 32;
    public int TotalPageCount { get; set; }
    public int StartPage { get; set; }
    public int EndPage { get; set; }
    public int DisplayRange { get; set; } = 5;
    public string? RouteName { get; set; }
    public int CheepMaxLength { get; set; } = 160;


    [BindProperty, StringLength(160), Required]
    public string? CheepMessage { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository, IValidator<CheepDto> validator)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        _validator = validator;
    }

    public ActionResult OnGet(string author)
    {
        //The following if statement has been made with the help of CHAT-GPT
        if (Request.Query.TryGetValue("page", out var pageValues) && int.TryParse(pageValues, out int parsedPage) && parsedPage > 0)
        {
            CurrentPage = parsedPage;
        }

        Cheeps = _cheepRepository.GetCheepsFromAuthorPage(author, CurrentPage);

        if (GetTotalPages(author) == 0)
        {
            TotalPageCount = 1;
        } else {
            TotalPageCount = GetTotalPages(author);
        }
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

    public ActionResult OnPostChirp()
    {
        /*if (CheepMessage is not null && CheepMessage.Length > 160) //TODO Enters accounts for 2 characters
        {
            throw new ArgumentException($"Message cannot be longer than 160 characters. Message was: {CheepMessage.Length} characters long");
        }*/

        try
        {
            if (User.Identity?.Name != null)
            {
                var author = new AuthorDto
                {
                    Name = User.Identity.Name, //Might need to be changed to use only User.Identity (Does not work until users are implemented)
                    Email = User.Identity.Name + "@chirp.com" //TODO: Needs to be removed
                };

                _authorRepository.CreateAuthor(author);
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }

        // Convert the time zone to Copenhagen 
        TimeZoneInfo copenhagenTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
        DateTime copenhagenTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, copenhagenTimeZone);

        var cheep = new CheepDto
        {
            Message = CheepMessage,
            TimeStamp = copenhagenTime.ToString(),
            AuthorName = User.Identity?.Name ?? "Anonymous"
        };
        
        ValidationResult result = _validator.Validate(cheep);

        if (result.IsValid) _cheepRepository.CreateCheep(cheep);

        
        return RedirectToPage("Public");
    }    
}
