using System.Globalization;

namespace ITU_BDSA23_GROUP27.Chirp.CLI;

public static class Utility
{
    internal const string DATE_FORMAT = "MM/dd/yy HH:mm:ss";

    public static long DateToTimestamp(string datetime)
    {
        if (!DateTime.TryParseExact(datetime, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datetimeParse))
        {
            return 0;
        }
        
        long timestamp = new DateTimeOffset(datetimeParse.ToUniversalTime()).ToUnixTimeSeconds();
        return timestamp;
    }
    
    public static string TimestampToDate(string timestamp)
    {
        if (!int.TryParse(timestamp, out int unixTimestamp))
        {
            throw new ArgumentException("Invalid Timestamp");
        }

        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        return dateTimeOffset.ToLocalTime().ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
    }
}