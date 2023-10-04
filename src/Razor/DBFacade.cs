using Microsoft.Data.Sqlite;

namespace Razor;

public class DBFacade
{
    private const int PageLength = 32;
    
    private static DBFacade? instance;
    private string? CHIRPDBPATH;
    
    public static DBFacade Instance
    {
        get
        {
            if (instance is not null)
            {
                return instance;
            }

            instance = new DBFacade();
            return instance;
        }
    }

    private SqliteConnection connection;

    public DBFacade()
    {
        /*CHIRPDBPATH = Environment.GetEnvironmentVariable("CHIRPDBPATH", EnvironmentVariableTarget.Machine);
        Console.WriteLine(CHIRPDBPATH);
        
        if (CHIRPDBPATH is null)
        {
            Console.WriteLine("Environment Variable 'CHIRPDBPATH' Does Not Exist");
            CHIRPDBPATH = Path.GetTempPath() + "chirp.db";
        }*/
        
        connection = new SqliteConnection("Data Source=./chirp.db");
        connection.Open();

        /*if (new FileInfo(CHIRPDBPATH).Length == 0)
        {
            WriteToEmptyDB();
        }*/
    }

    private void WriteToEmptyDB()
    {
        var command = connection.CreateCommand();
        
        using (var reader = new StreamReader("../SQLite/data/schema.sql"))
        {
            command.CommandText = reader.ReadToEnd();
        }
        command.ExecuteNonQuery();
        
        using (var reader = new StreamReader("../SQLite/data/dump.sql"))
        {
            command.CommandText = reader.ReadToEnd();
        }
        command.ExecuteNonQuery();
    }
    
    public List<CheepViewModel> ReadCheeps(int page)
    {
        var command = connection.CreateCommand();

        command.CommandText =
            @"
                SELECT U.username, M.text, M.pub_date
                FROM message M
                JOIN user U ON M.author_id = U.user_id
                LIMIT 32 OFFSET @page*@PageLength
            ";
        
        command.Parameters.AddWithValue("@page", page);
        command.Parameters.AddWithValue("@PageLength", PageLength);
        
        List<CheepViewModel> cheeps = new List<CheepViewModel>();
        
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                
                // could probably be refactored
                var author = reader.GetString(0);
                var message = reader.GetString(1);
                var timestamp = reader.GetString(2);

                cheeps.Add(new CheepViewModel(author,message, UnixTimeStampToDateTimeString(Convert.ToDouble(timestamp))));
            }
        }

        return cheeps;
    }
    
    public List<CheepViewModel> ReadCheepsFromAuthor(string author, int page)
    {
        var command = connection.CreateCommand();

        command.CommandText =
            @"
                SELECT M.text, M.pub_date
                FROM message M
                JOIN user U ON M.author_id = U.user_id
                WHERE U.username = @Author
                LIMIT 32 OFFSET @page*@PageLength
            ";

        command.Parameters.AddWithValue("@Author", author);
        command.Parameters.AddWithValue("@page", page);
        command.Parameters.AddWithValue("@PageLength", PageLength);
        
        List<CheepViewModel> cheeps = new List<CheepViewModel>();
        
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                
                // could probably be refactored
                var message = reader.GetString(0);
                var timestamp = reader.GetString(1);

                cheeps.Add(new CheepViewModel(author,message, UnixTimeStampToDateTimeString(Convert.ToDouble(timestamp))));
            }
        }

        return cheeps;
    }
    
    // Could be in an utility class
    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
    
    
}