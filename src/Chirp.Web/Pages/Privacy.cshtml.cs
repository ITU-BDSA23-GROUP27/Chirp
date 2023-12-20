using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.Pages;

public class PrivacyModel : BasePageModel
{
    public ILogger<PrivacyModel> Logger => _logger;

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

