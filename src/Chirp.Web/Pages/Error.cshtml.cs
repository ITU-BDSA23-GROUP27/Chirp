using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
namespace Chirp.Web.Pages;

/// <summary>
/// PageModel for the Error page that is shown when an error occurs.
/// </summary>

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : BasePageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public Task OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        return Task.CompletedTask;
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

