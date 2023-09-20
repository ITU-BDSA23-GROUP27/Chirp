namespace SimpleDB.Tests;

public class IntegrationTests
{
    
    [Fact]
    public void CSVDatabase_Cheep_Read_ReturnsLastCheep()
    {
        string filepath = "../../../../../data/chirp_cli_db.csv";
        IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>(filepath);
        
        Cheep entry = new Cheep(Environment.UserName, "message", 1690891760);
        database.Store(entry);

        var cheeps = database.Read().ToArray();
        var lastCheep = cheeps[^1];
    
        Assert.Equal("message", lastCheep.Message);
    }
    
    public record Cheep(string Author, string Message, long Timestamp);
}