using System.Net;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace CSVDBService.Tests;

public class IntegrationTests
{
    [Fact]
    public async void StatusCode_Cheeps_Is_200()
    {
        // HTTP Client Creation
        var baseURL = "http://localhost:5100";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);
        
        // Arrange
        var response = await client.GetAsync("/cheeps");
        
        // Act
        var statusCode = response.StatusCode;
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
    }
    
    [Fact]
    public async void StatusCode_Cheep_Is_200()
    {
        // HTTP Client Creation
        var baseURL = "http://localhost:5100";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);
        
        // Arrange
        var cheep = new Cheep(Environment.UserName, "message", 1690891760);
        var httpCheep = new StringContent(JsonConvert.SerializeObject(cheep), Encoding.UTF8, "application/json");
        
        // Act
        var response = await client.PostAsync("/cheep", httpCheep);
        var statusCode = response.StatusCode;
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
    }
    public record Cheep(string Author, string Message, long Timestamp);
}