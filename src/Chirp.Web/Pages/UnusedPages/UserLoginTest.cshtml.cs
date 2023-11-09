using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserLoginTestModel : PageModel
{
    public void OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            Console.WriteLine(User.Identity.Name);
        }
        else
        {
            Console.WriteLine("Not Authenticated");
        }
    }
}