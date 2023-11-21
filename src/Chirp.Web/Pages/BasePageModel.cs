using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class BasePageModel : PageModel
{
    protected IActionResult HandleNotAuthenticated()
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            return RedirectToPage("/Public");
        }

        return Page();
    }
}