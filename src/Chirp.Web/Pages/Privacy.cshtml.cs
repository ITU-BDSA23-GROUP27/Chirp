using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PrivacyModel : BasePageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }
    
    public IActionResult OnPostAuthenticateLogin()
    {
        return HandleAuthenticateLogin();
    }
    public IActionResult OnPostLogOut()
    {
        return HandleLogOut();
    }
}

