namespace Chirp.CLI.Tests;
using ITU_BDSA23_GROUP27.Chirp.CLI;

public class UtilityTests
{
    [Fact]
    public void DateToTimestampTest()
    {
        long actual = Utility.DateToTimestamp("08/01/23 14:09:20");

        long expected = 1690891760;
        
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void TimestampToDateTest()
    {
        string actual = Utility.TimestampToDate("1690891760");

        string expected = "08/01/23 14:09:20";
        
        Assert.Equal(expected, actual);
    }
}