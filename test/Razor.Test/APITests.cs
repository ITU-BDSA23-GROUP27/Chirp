using AngleSharp;
using AngleSharp.Dom;

namespace Razor.Test;

public class APITests
{
    [Theory]
    [InlineData("?page=20", "Helge", "Hello, BDSA students!")]
    [InlineData("Rasmus", "Rasmus", "Hej, velkommen til kurset.")]
    public async void GetCheepTest(string query, string name, string message)
    {
        string url = $"https://bdsagroup27chirprazor.azurewebsites.net/{query}";
        var document = await WebCrawler.DownloadDocument(url);
        Assert.NotNull(document);
        
        var messageList = document.QuerySelector("#messagelist");
        Assert.NotNull(messageList);
        
        var containsCheep = messageList.Children.Any(element => 
            element.QuerySelector("> p > strong").TextContent.Contains(name) &&
            element.QuerySelector("> p").TextContent.Contains(message));
        
        Assert.True(containsCheep);
    }
}