namespace Chirp.CLI.Tests;
using ITU_BDSA23_GROUP27.Chirp.CLI;

public class UnitTests
{
    
    [Theory]
    [InlineData("08/01/23 14:09:20", 1690891760)]
    [InlineData("08/02/23 14:19:38", 1690978778)]
    [InlineData("08/02/23 14:37:38", 1690979858)]
    [InlineData("08/02/23 15:04:47", 1690981487)]
    public void DateToTimestamp_Convert_ReturnsTimestamp(string input, long expected)
    {
        // Act
        long actual = Utility.DateToTimestamp(input);
        
        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("1690891760", "08/01/23 14:09:20")]
    [InlineData("1690978778", "08/02/23 14:19:38")]
    [InlineData("1690979858", "08/02/23 14:37:38")]
    [InlineData("1690981487", "08/02/23 15:04:47")]
    public void TimestampToDate_Convert_ReturnsDate(string input, string expected)
    {
        // Act
        string actual = Utility.TimestampToDate(input);
        
        // Assert
        Assert.Equal(expected, actual);
    }
}