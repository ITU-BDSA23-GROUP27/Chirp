using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace CSVDBService.Tests;

public class IntegrationTests
{
    [Theory]
    [InlineData("cheep cheep")]
    [InlineData("test-cheep is stored and retrieved from a temp.csv")]
    public async Task CSVDatabase_CheepAndRead_ReturnsLastCheep(string input)
    {
        // Arrange - Create Client
        var baseURL = "https://bdsagroup27chirpremotedb.azurewebsites.net/";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);
        
        // Act - Post/Store Cheep
        Cheep cheep = new Cheep(Environment.UserName, input, 1690891760);
        var http = new StringContent(JsonConvert.SerializeObject(cheep), Encoding.UTF8, "application/json");
        await client.PostAsync("/cheep", http);
        
        // Act - Read Cheeps
        var response = await client.GetAsync("/cheeps");
        var responseContent = await response.Content.ReadAsStringAsync();
        var cheeps = JsonConvert.DeserializeObject<List<Cheep>>(responseContent).ToArray();
        
        var lastCheep = cheeps[^1];

        string expected = input;
        string actual = lastCheep.Message;
        
        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CSVDatabase_ReadFirstCheep_ReturnsFirstCheep()
    {
        // Arrange - Create Client
        var baseURL = "https://bdsagroup27chirpremotedb.azurewebsites.net/";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);
        
        // Act
        var response = await client.GetAsync("/cheeps");
        var responseContent = await response.Content.ReadAsStringAsync();
        var cheeps = JsonConvert.DeserializeObject<List<Cheep>>(responseContent).ToArray();

        var firstCheep = cheeps[0];

        string expected = "Hello, BDSA students!";
        string actual = firstCheep.Message;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task CSVDatabase_ReadFourthAuthor_ReturnsFourthAuthor()
    {
        // Arrange - Create Client
        var baseURL = "https://bdsagroup27chirpremotedb.azurewebsites.net/";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);
        
        // Act
        var response = await client.GetAsync("/cheeps");
        var responseContent = await response.Content.ReadAsStringAsync();
        var cheeps = JsonConvert.DeserializeObject<List<Cheep>>(responseContent).ToArray();

        var fourthCheep = cheeps[3];

        string expected = "ropf";
        string actual = fourthCheep.Author;

        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task StatusCode_Cheeps_Is_200()
    {
        // HTTP Client Creation
        var baseURL = "https://bdsagroup27chirpremotedb.azurewebsites.net/";
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
    public async Task StatusCode_Cheep_Is_200()
    {
        // HTTP Client Creation
        var baseURL = "https://bdsagroup27chirpremotedb.azurewebsites.net/";
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