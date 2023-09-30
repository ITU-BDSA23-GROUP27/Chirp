using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Chirp.CSVDBService.SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private static CSVDatabase<T>? instance;

    public static CSVDatabase<T> Instance
    {
        get
        {
            return instance ??= new CSVDatabase<T>();
        }
    }

    private const string FILE = "data/chirp_cli_db.csv";
    
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