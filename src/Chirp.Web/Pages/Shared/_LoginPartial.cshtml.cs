using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Shared;

public class LoginPartialModel : PageModel
{
    public IResult OnGetAuthenticateLogin()
    {
        return Results.Challenge(new AuthenticationProperties(){RedirectUri = "http://localhost:5273/login"}, new List<string>(){"github"});
    }
}