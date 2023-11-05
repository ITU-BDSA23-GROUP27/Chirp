using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Areas.Identity.Pages;

[Authorize]
public class auth : PageModel
{
    public void OnGet()
    {
        
    }
}