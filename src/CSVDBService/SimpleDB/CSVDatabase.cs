﻿using System.Globalization;
using Chirp.CSVDBService.SimpleDB.Utility;
using CsvHelper;
using CsvHelper.Configuration;

namespace Chirp.CSVDBService.SimpleDB;

public class CSVDatabase<T> : Singleton<CSVDatabase<T>>, IDatabaseRepository<T>
{
    private readonly string FILE;

    public CSVDatabase()
    {
        FILE = "data/chirp_cli_db.csv";
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