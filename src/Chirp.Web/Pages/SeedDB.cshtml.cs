using Chirp.Infrastructure;
using Chirp.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Web.Pages;

// TODO - expand with user manager role - [Authorize(Roles = "Admin")]
[Authorize]
public class SeedDbModel : BasePageModel
{
    private readonly ChirpContext _context;
    
    public SeedDbModel(ChirpContext context)
    {
        _context = context;
    }

    // button for seeding DbInitializer
    public async Task<IActionResult> OnPostSeedDatabase()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.MigrateAsync();
        DbInitializer.SeedDatabase(_context);
        return RedirectToPage("/Public");
    }

    // button for seeding DbInitializer2 (authors with followers)
    public async Task<IActionResult> OnPostSeedDatabase2()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.MigrateAsync();
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
    
    // public async Task<IActionResult> OnGet()
    // {
    //     return await HandleNotAuthenticated();
    // }

    public async Task<IActionResult> OnPostLogOut()
    {
        return await HandleLogOut();
    }
}