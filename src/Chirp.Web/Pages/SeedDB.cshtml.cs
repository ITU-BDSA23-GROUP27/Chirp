using Chirp.Infrastructure;
using Chirp.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web.Pages;
public class SeedDBModel : BasePageModel
{
    private readonly ChirpContext _context;
    
    public SeedDBModel(ChirpContext context)
    {
        _context = context;
    }
    
    public IActionResult OnGet()
    {
        return HandleNotAuthenticated();
    }

    public IActionResult OnPostSeedDatabase()
    {
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
        DbInitializer.SeedDatabase(_context);
        return RedirectToPage("/Public");
    }
    
    public IActionResult OnPostClearDatabase()
    {
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
        return RedirectToPage("/Public");
    }

    public IActionResult OnPostLogOut()
    {
        return HandleLogOut();
    }
}