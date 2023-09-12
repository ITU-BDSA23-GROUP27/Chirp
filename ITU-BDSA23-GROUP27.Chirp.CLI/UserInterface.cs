using System.Globalization;
namespace ITU_BDSA23_GROUP27.Chirp.CLI;

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps, int? limit)
    {
        var list = cheeps.ToList();
        int readLimit = limit ?? list.Count;

        if (readLimit <= 0)
        {
            throw new ArgumentException("limit must be null or greater than 0");
        }
        
        int counter = 0;
        
        foreach ((string author, string message, long timestamp) in list)
        {
            if (counter == readLimit)
            {
                break;
            }
            
            string chirp = $"{author} @ {TimestampToDate(timestamp.ToString())}: {message}";
            Console.WriteLine(chirp);
            counter++;
        }
    }
    
    private static string TimestampToDate(string timestamp)
    {
        if (!int.TryParse(timestamp, out int unixTimestamp))
        {
            throw new ArgumentException("Invalid Timestamp");
        }

        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        return dateTimeOffset.ToLocalTime().ToString(Utility.DATE_FORMAT, CultureInfo.InvariantCulture);
    }
}