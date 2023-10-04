using AngleSharp;
using AngleSharp.Dom;

namespace Razor.Test;

public static class WebpageDownloader
{
    internal static async Task<IDocument> DownloadDocument(string url)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync($"{url}");

        return document;
    }
}