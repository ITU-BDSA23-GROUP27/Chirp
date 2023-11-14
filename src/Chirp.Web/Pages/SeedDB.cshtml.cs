using Chirp.Infrastructure;
using Chirp.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web.Pages;
public class SeedDBModel : PageModel
{
    private readonly ChirpContext _context;
    
    public SeedDBModel(ChirpContext context)
    {
        _context = context;
    }
    
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            return RedirectToPage("/Public");
        }        
        return Page();
    }

    public IActionResult OnPostSeedDatabase()
    {
        DbInitializer.SeedDatabase(_context);
        return Page();
    }
    
    public IActionResult OnPostClearDatabase()
    {
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
        return Page();
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
}