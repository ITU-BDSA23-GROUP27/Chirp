using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string FILE;
    
    public CSVDatabase(string FILE = "../../data/chirp_cli_db.csv")
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