﻿using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SimpleDB.Utility;

namespace SimpleDB;

public class CSVDatabase<T> : Singleton<CSVDatabase<T>>, IDatabaseRepository<T>
{
    private readonly string FILE;

    public CSVDatabase()
    {
        if (File.Exists("../../data/chirp_cli_db.csv"))
        {
            FILE = "../../data/chirp_cli_db.csv";
        } else {
            FILE  = Directory.GetCurrentDirectory() + "/chirp_cli_db.csv";

            if (File.Exists(FILE)) return;
            
            using var writer = new StreamWriter(FILE);
            // Write header row
            writer.WriteLine("Author,Message,Timestamp");
        }   
    }
    
    public CSVDatabase(string FILE) 
    {
        this.FILE = FILE;
    }
    
    public IEnumerable<T> Read(int? limit = null)
    {   
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using var reader = new StreamReader(FILE);
        using var csv = new CsvReader(reader, config);
        
        return csv.GetRecords<T>().ToList();
    }

    public void Store(T record)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using var writer = new StreamWriter(File.Open(FILE, FileMode.Append));
        using var csv = new CsvWriter(writer, config);
        
        csv.WriteRecord(record);
        writer.WriteLine();
    }
}