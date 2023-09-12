using System.Globalization;

namespace ITU_BDSA23_GROUP27.Chirp.CLI;

internal static class Utility
{
    internal const string DATE_FORMAT = "MM/dd/yy HH:mm:ss";

    internal static long DateToTimestamp(string datetime)
    {
        if (!DateTime.TryParseExact(datetime, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datetimeParse))
        {
            return 0;
        }

        long timestamp = new DateTimeOffset(datetimeParse).ToUnixTimeSeconds();
        return timestamp;
    }
}