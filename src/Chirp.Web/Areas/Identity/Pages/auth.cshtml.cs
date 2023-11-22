using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Areas.Identity.Pages;

[Authorize]
public class Auth : PageModel
{
    public void OnGet()
    {
        
    }
}