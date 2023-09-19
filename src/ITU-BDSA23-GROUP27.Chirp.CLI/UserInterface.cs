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
            
            string chirp = $"{author} @ {Utility.TimestampToDate(timestamp.ToString())}: {message}";
            Console.WriteLine(chirp);
            counter++;
        }
    }
}