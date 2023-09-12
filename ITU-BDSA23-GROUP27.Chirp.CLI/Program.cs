using System.Globalization;
using ITU_BDSA23_GROUP27.Chirp.CLI;
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
        UserInterface.PrintCheeps(database.Read());
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

void Cheep(string message)
{
    long timestamp = DateToTimestamp(DateTime.Now.ToString(DATE_FORMAT, CultureInfo.InvariantCulture));
    var record = new Cheep(Environment.UserName, message, timestamp);
    database.Store(record);
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