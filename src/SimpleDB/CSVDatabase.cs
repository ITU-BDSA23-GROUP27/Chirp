using System.Globalization;
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

            if (!File.Exists(FILE)) //creates a .csv file if not found
            {
                Console.WriteLine("No .cvs file was found! An empty cvs. file will be created at the current directory");
                
                using var writer = new StreamWriter(FILE);
                writer.WriteLine("Author,Message,Timestamp"); // Write header row
            }
            
            //instructs user to cheep before reading
            if (IsCsvFileEmpty(FILE)) Console.WriteLine("There's no Cheeps! Add some Cheeps by using the command: cheep -m \"<message>\"");
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
    
    static bool IsCsvFileEmpty(string filePath)
    {
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Read the header row
                reader.ReadLine();

                // Check if the file ends after the header
                return reader.EndOfStream;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false; // Handle the error as needed
        }
    }
}