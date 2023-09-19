namespace SimpleDB.Tests;

public class DatabaseIntegrationTests
{
    
    [Fact]
    public void EntryReceived()
    {
        IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();
        
        Cheep entry = new Cheep(Environment.UserName, "message", 1690891760);
        database.Store(entry);

        var cheeps = database.Read().ToList();
        var lastCheep = cheeps[cheeps.Count];
    
        Assert.Equal("message", lastCheep.Message);
    }
    
    public record Cheep(string Author, string Message, long Timestamp);
}