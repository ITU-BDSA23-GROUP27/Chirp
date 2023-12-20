using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

/// <summary>
/// PageModel for the Privacy page that shows the privacy policy of the application.
/// Both authenticated and unauthenticated users can access the Privacy page
/// </summary>

public class PrivacyModel : BasePageModel
{
    public async Task<IActionResult> OnPostAuthenticateLogin()
    {
        return await HandleAuthenticateLogin();
    }
    public async Task<IActionResult> OnPostLogOut()
    {
        return await HandleLogOut();
    }
}

