using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

/// <summary>
/// PageModel for the Privacy page that shows the privacy policy of the application.
/// Both authenticated and unauthenticated users can access the Privacy page
/// </summary>

public class PrivacyModel : BasePageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }
    
    public async Task<IActionResult> OnPostAuthenticateLogin()
    {
        return await HandleAuthenticateLogin();
    }
    public async Task<IActionResult> OnPostLogOut()
    {
        return await HandleLogOut();
    }
}

