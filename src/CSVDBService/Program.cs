using Chirp.CSVDBService.SimpleDB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Read cheeps
app.MapGet("/cheeps", () => CSVDatabase<Cheep>.Instance.Read());

//Store cheep
app.MapPost("/cheep", (Cheep cheep) => CSVDatabase<Cheep>.Instance.Store(cheep));

app.Run();

public record Cheep(string Author, string Message, long Timestamp);