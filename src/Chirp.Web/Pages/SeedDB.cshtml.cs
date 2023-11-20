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

    // button for seeding DbInitializer
    public IActionResult OnPostSeedDatabase()
    {
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
        DbInitializer.SeedDatabase(_context);
        return RedirectToPage("/Public");
    }

    // button for seeding DbInitializer2 (authors with followers)
    public IActionResult OnPostSeedDatabase2()
    {
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
        DbInitializer2.SeedDatabase2(_context);
        return RedirectToPage("/Public");
    }
    
    // button for deleting all data from database (table remains)
    public IActionResult OnPostClearDatabase()
    {
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
        return RedirectToPage("/Public");
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