using System.Globalization;
using CommandLine;
using ITU_BDSA23_GROUP27.Chirp.CLI;
using SimpleDB;

const string DATE_FORMAT = "MM/dd/yy HH:mm:ss";
IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

const string usage = @"Missing arguments:
  (--r | --read) <limit>      : Read all cheeps
  (--c | --cheep) <message>   : Create a new cheep

  --h                         : Show this help screen
";

Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
{
    if (o.Read)
    {
        UserInterface.PrintCheeps(database.Read(), o.ReadLimit);
    }
    else if (o.Cheep is not null)
    {
        Cheep(o.Cheep);
    }
    else if (o.Help)
    {
        Console.WriteLine(usage);
    }
    else
    {
        Console.WriteLine(usage);
    }
});

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

public class Options
{
    [Option('r', "read", Required = false, HelpText = "Read all cheeps")]
    public bool Read { get; set; }
    
    [Value(1, Required = false, HelpText = "Value for read limit")]
    public long? ReadLimit { get; set; }
    
    [Option('c', "cheep", Required = false, HelpText = "Create a new Cheep")]
    public string? Cheep { get; set; }
    
    [Option('h', "help", Required = false, HelpText = "Display a help screen")]
    public bool Help { get; set; }
}