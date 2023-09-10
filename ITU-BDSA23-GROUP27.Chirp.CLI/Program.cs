﻿using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

const string FILE = "chirp_cli_db.csv";
const string DATE_FORMAT = "MM/dd/yy HH:mm:ss";

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
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
    };
    using (var reader = new StreamReader(FILE))
    using (var csv = new CsvReader(reader, config))
    {
        var records = csv.GetRecords<Cheep>();

        foreach (var (author, message, timestamp) in records)
        {
            string chirp = $"{author} @ {TimestampToDate(timestamp.ToString())}: {message}";
            Console.WriteLine(chirp);
        }
    }
    /*
    string[] lines = File.ReadAllLines(FILE);

    for (int i = 1; i < lines.Length; i++) // skip header line
    {
        string[] data = lines[i].Split('"');

        string author = data[0].TrimEnd(',');
        string message = data[1];
        string timestamp = data[2].TrimStart(',');

        string datetime = TimestampToDate(timestamp);

        string chirp = $"{author} @ {datetime}: {message}";
        Console.WriteLine(chirp);
    }*/
}

void Cheep(string message)
{
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
    };
    using (var writer = new StreamWriter(File.Open(FILE,FileMode.Append)))
    using (var csv = new CsvWriter(writer, config))
    {
        long timestamp = DateToTimestamp(DateTime.Now.ToString(DATE_FORMAT, CultureInfo.InvariantCulture));
        var record = new Cheep(Environment.UserName, message, timestamp);
        csv.WriteRecord(record);
        writer.WriteLine();
    }
    /*
    string username = Environment.UserName;
    long timestamp = DateToTimestamp(DateTime.Now.ToString(DATE_FORMAT, CultureInfo.InvariantCulture));

    string chirp = $"{username},\"{message}\",{timestamp}";
    
    File.AppendAllText(FILE, chirp + Environment.NewLine);

    Console.WriteLine("Text appended successfully.");
    */
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