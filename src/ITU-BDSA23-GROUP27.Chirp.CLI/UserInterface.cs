namespace ITU_BDSA23_GROUP27.Chirp.CLI;

public static class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> ?cheeps, int? limit)
    {
        if (cheeps is null) { return; }
        
        var list = cheeps.ToList();
        int readLimit = limit ?? list.Count;

        //Error handling
        if (list.Count == 0)
        {
            throw new ArgumentException("There are no cheeps stored");
        }
        if (readLimit < 0)
        {
            throw new ArgumentException("Limit must be a positive integer");
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