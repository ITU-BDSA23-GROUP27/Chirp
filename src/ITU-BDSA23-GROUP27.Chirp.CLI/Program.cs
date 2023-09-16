using System.Globalization;
using CommandLine;
using ITU_BDSA23_GROUP27.Chirp.CLI;
using SimpleDB;

var types = CommandLineOptions.GetVerbs();            

Parser.Default.ParseArguments(args, types)
    .WithParsed(Run)
    .WithNotParsed(HandleErrors);

static void HandleErrors(object obj)
{
    // TODO Implement error handling
}
        
static void Run(object obj)
{
    switch (obj)
    {
        case CommandLineOptions.CheepOptions c:
            Cheep(c.Message);
            break;
        case CommandLineOptions.ReadOptions r:
            ReadCheeps(r.Limit);
            break;
        case CommandLineOptions.Options a:
            break;
    }
}

static void Cheep(string message)
{
    IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

    long timestamp = Utility.DateToTimestamp(DateTime.Now.ToString(Utility.DATE_FORMAT, CultureInfo.InvariantCulture));
    Cheep record = new Cheep(Environment.UserName, message, timestamp);
    database.Store(record);
}

static void ReadCheeps(int? limit)
{
    IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();
    UserInterface.PrintCheeps(database.Read(), limit);
}

public record Cheep(string Author, string Message, long Timestamp);