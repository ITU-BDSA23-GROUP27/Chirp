namespace SimpleDB.Tests;
using Xunit.Abstractions;

public class IntegrationTests
{

    private readonly string testCsvFilePath;
    private readonly CSVDatabase<Cheep> database;
    private readonly ITestOutputHelper _testOutputHelper;

    public IntegrationTests(ITestOutputHelper testOutputHelper)
    {
        // Log test output to the console
        _testOutputHelper = testOutputHelper;

        // Create a temporary test CSV file for the integration tests
        testCsvFilePath = Path.Combine(Path.GetTempPath(), "test_chirp_cli_db.csv");

        // Copy the data file from the specific path to the temporary location
        string sourceDataFilePath = "../../../../../data/chirp_cli_db.csv";
        File.Copy(sourceDataFilePath, testCsvFilePath, true);

        // Initialize the database using the test CSV file
        database = new CSVDatabase<Cheep>(testCsvFilePath);
    }

    [Theory]
    [InlineData("cheep cheep")]
    [InlineData("test-cheep is stored and retrieved from a temp.csv")]
    public void CSVDatabase_CheepAndRead_ReturnsLastCheep(string input)
    {
        // Arrange
        Cheep cheep = new Cheep(Environment.UserName, input, 1690891760);

        // Act
        database.Store(cheep);

        var cheeps = database.Read().ToArray();
        var lastCheep = cheeps[^1];

        string expected = input;
        string actual = lastCheep.Message;

        // _testOutputHelper.WriteLine(cheeps[^1].Message);

        // Assert
        Assert.Equal(expected, actual);
    
        // Clean up the temporary file after tests are finished
        File.Delete(testCsvFilePath);
    }

    [Fact]
    public void CSVDatabase_ReadFirstCheep_ReturnsFirstCheep()
    {
        // Act
        var cheeps = database.Read().ToArray();

        var firstCheep = cheeps[0];

        string expected = "Hello, BDSA students!";
        string actual = firstCheep.Message;

        // Assert
        Assert.Equal(expected, actual);

        File.Delete(testCsvFilePath);
    }

    [Fact]
    public void CSVDatabase_ReadFourthAuthor_ReturnsFourthAuthor()
    {
        // Act
        var cheeps = database.Read().ToArray();

        var fourthCheep = cheeps[3];

        string expected = "ropf";
        string actual = fourthCheep.Author;

        // Assert
        Assert.Equal(expected, actual);

        File.Delete(testCsvFilePath);
    }

    public record Cheep(string Author, string Message, long Timestamp);
}