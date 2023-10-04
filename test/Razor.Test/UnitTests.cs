namespace Razor.Test;

public class UnitTests
{
    [Theory]
    [InlineData(1690891760, "08/01/23 12.09.20")]
    [InlineData(1690978778, "08/02/23 12.19.38")]
    [InlineData(1690979858, "08/02/23 12.37.38")]
    [InlineData(1690981487, "08/02/23 13.04.47")]
    public void TimestampToDate_Convert_ReturnsDate(double input, string expected)
    {
        // Act
        string actual = DBFacade.UnixTimeStampToDateTimeString(input);
        
        // Assert
        Assert.Equal(expected, actual);
    }
}