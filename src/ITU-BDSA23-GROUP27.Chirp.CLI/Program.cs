using System.Globalization;
using CommandLine;
using ITU_BDSA23_GROUP27.Chirp.CLI;
using System.Text;
using Newtonsoft.Json;

var types = CommandLineOptions.GetVerbs();

// HTTP Client Creation
var baseURL = "http://localhost:5100";
using HttpClient client = new();
client.BaseAddress = new Uri(baseURL);

Parser.Default.ParseArguments(args, types)
    .WithParsed(Run)
    .WithNotParsed(HandleErrors);

static void HandleErrors(object obj)
{
    // TODO Implement error handling
}
        
void Run(object obj)
{
    switch (obj)
    {
        case CommandLineOptions.CheepOptions c when c.Message != null:
            Cheep(c.Message).Wait();
            break;
        case CommandLineOptions.ReadOptions r:
            ReadCheeps(r.Limit).Wait();
            break;
        case CommandLineOptions.Options a:
            break;
    }
}

async Task Cheep(string message)
{
    long timestamp = Utility.DateToTimestamp(DateTime.Now.ToString(Utility.DATE_FORMAT, CultureInfo.InvariantCulture));
    Cheep record = new Cheep(Environment.UserName, message, timestamp);
    var http = new StringContent(JsonConvert.SerializeObject(record), Encoding.UTF8, "application/json");
    await client.PostAsync("/cheep", http);
}

async Task ReadCheeps(int? limit)
{
    var response = await client.GetAsync("/cheeps");
    var responseContent = await response.Content.ReadAsStringAsync();
    IEnumerable<Cheep> ?cheeps = JsonConvert.DeserializeObject<List<Cheep>>(responseContent);
    
    UserInterface.PrintCheeps(cheeps, limit);
}

public record Cheep(string Author, string Message, long Timestamp);