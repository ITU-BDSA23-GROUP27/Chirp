using AngleSharp;
using AngleSharp.Dom;

namespace Razor.Test;

public class APITests
{
    /*
    [Theory]
    [InlineData("?page=20", "Helge", "Hello, BDSA students!")]
    [InlineData("Rasmus", "Rasmus", "Hej, velkommen til kurset.")]
    public async void GetCheepTest(string query, string name, string message)
    {
        string url = $"https://bdsagroup27chirprazor.azurewebsites.net/{query}";
        var document = await WebpageDownloader.DownloadDocument(url);
        Assert.NotNull(document);
        
        var messageList = document.QuerySelector("#message-list");
        Assert.NotNull(messageList);
        
        var containsCheep = messageList.Children.Any(element => 
            element.QuerySelector("#author").TextContent.Contains(name) &&
            element.QuerySelector("#cheep-content").TextContent.Contains(message));
        
        Assert.True(containsCheep);
    }
    */
}