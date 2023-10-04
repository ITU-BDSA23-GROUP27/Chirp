using AngleSharp;
using AngleSharp.Dom;

namespace Razor.Test;

public class WebCrawler
{
    internal static async Task<IDocument> DownloadDocument(string url)
    {
        var config = Configuration.Default.WithDefaultLoader();
        var context = BrowsingContext.New(config);

        var document = await context.OpenAsync($"{url}");
        if (document == null)
        {
            throw new Exception("Document is null");
        }

        return document;
    }
}