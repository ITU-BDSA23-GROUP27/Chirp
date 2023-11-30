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
    private readonly ChirpDbContext dbContext;
    
    public SeedDbModel(ChirpDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // button for seeding DbInitializer
    public async Task<IActionResult> OnPostSeedDatabase()
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
        DbInitializer.SeedDatabase(dbContext);
        return RedirectToPage("/Public");
    }

    // button for seeding DbInitializer2 (authors with followers)
    public async Task<IActionResult> OnPostSeedDatabase2()
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
        DbInitializer2.SeedDatabase2(dbContext);
        return RedirectToPage("/Public");
    }
    
    // button for deleting all data from database (table remains)
    public IActionResult OnPostClearDatabase()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();
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