using System.Globalization;
namespace ITU_BDSA23_GROUP27.Chirp.CLI;

public class UserInterface
{
    const string DATE_FORMAT = "MM/dd/yy HH:mm:ss";
    
    public static void PrintCheeps(IEnumerable<Cheep> cheeps, long? limit)
    {
        var list = cheeps.ToList();
        
        switch (limit)
        {
            case null:
                limit = list.Count;
                break;
            
            case <= 0:
                throw new ArgumentException("Limit cannot be less than or equal to zero");
        }
        
        int i = 0;
        foreach (var (author, message, timestamp) in list)
        {
            if (i == limit) break;
            string chirp = $"{author} @ {TimestampToDate(timestamp.ToString())}: {message}";
            Console.WriteLine(chirp);
            i++;
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