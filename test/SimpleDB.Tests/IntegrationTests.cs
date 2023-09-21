using System.Text;
using Newtonsoft.Json;

namespace SimpleDB.Tests;

public class IntegrationTests
{
    [Fact]
    public async void CSVDatabase_Cheep_Read_ReturnsLastCheep()
    {
        // HTTP Client Creation
        var baseURL = "http://localhost:5100";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);
        
        // Arrange
        Cheep entry = new Cheep(Environment.UserName, "message", 1690891760);
        var httpCheep = new StringContent(JsonConvert.SerializeObject(entry), Encoding.UTF8, "application/json");
        
        // Act
        await client.PostAsync("/cheep", httpCheep);
        
        var cheepsHttp = await client.GetAsync("/cheeps");
        var cheepsHttpContent = await cheepsHttp.Content.ReadAsStringAsync();
        var cheepArray = JsonConvert.DeserializeObject<List<Cheep>>(cheepsHttpContent).ToArray();
        
        var lastCheep = cheepArray[^1];
    
        // Assert
        Assert.Equal("message", lastCheep.Message);
    }
    
    public record Cheep(string Author, string Message, long Timestamp);
}