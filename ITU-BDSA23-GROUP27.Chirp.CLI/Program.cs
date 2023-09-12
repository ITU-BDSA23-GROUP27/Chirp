using System.Globalization;
using SimpleDB;

const string DATE_FORMAT = "MM/dd/yy HH:mm:ss";
IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

if (args.Length < 1)
{
    Console.WriteLine("Usage: \n  " +
                      " - dotnet <read> \n  " +
                      " - dotnet <cheep> [message]");
    return;
}

switch (args[0])
{
    case "read":
        ReadChirps();
        break;
    case "cheep" when args.Length < 2:
        Console.WriteLine("A cheep message is missing!");
        return;
    case "cheep":
        Cheep(args[1]);
        break;
    default:
        Console.WriteLine("Invalid command. Use 'read' or 'cheep'.");
        break;
}

void ReadChirps()
{
    var records = database.Read();
    foreach (var (author, message, timestamp) in records)
    {
        string chirp = $"{author} @ {TimestampToDate(timestamp.ToString())}: {message}";
        Console.WriteLine(chirp);
    }
}

void Cheep(string message)
{
    long timestamp = DateToTimestamp(DateTime.Now.ToString(DATE_FORMAT, CultureInfo.InvariantCulture));
    var record = new Cheep(Environment.UserName, message, timestamp);
    database.Store(record);
}

string TimestampToDate(string timestamp)
{
    if (int.TryParse(timestamp, out int unixTimestamp))
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
        string formattedDateTime = dateTimeOffset.ToLocalTime().ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
        return formattedDateTime;
    }

    return "Invalid Timestamp";
}

long DateToTimestamp(string datetime)
{
    if (DateTime.TryParseExact(datetime, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datetimeParse))
    {
        long timestamp = new DateTimeOffset(datetimeParse).ToUnixTimeSeconds();
        return timestamp;
    }

    return 0;
}

public record Cheep(string Author, string Message, long Timestamp);