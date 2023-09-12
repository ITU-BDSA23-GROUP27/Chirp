using System.Globalization;
namespace ITU_BDSA23_GROUP27.Chirp.CLI;

public class UserInterface
{
    const string DATE_FORMAT = "MM/dd/yy HH:mm:ss";
    
    public static void PrintCheeps(IEnumerable<Cheep> Cheeps)
    {
        foreach (var (author, message, timestamp) in Cheeps)
        {
            string chirp = $"{author} @ {TimestampToDate(timestamp.ToString())}: {message}";
            Console.WriteLine(chirp);
        }
    }
    
    private static string TimestampToDate(string timestamp)
    {
        if (int.TryParse(timestamp, out int unixTimestamp))
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
            string formattedDateTime = dateTimeOffset.ToLocalTime().ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
            return formattedDateTime;
        }

        return "Invalid Timestamp";
    }
}