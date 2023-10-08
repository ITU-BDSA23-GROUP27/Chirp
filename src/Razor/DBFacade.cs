using Microsoft.Data.Sqlite;

namespace Razor;

public class DBFacade
{
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
    
    private static DBFacade? instance;
    
    private const int PageLength = 32;
    private SqliteConnection connection;

    private DBFacade()
    {
        connection = new SqliteConnection("Data Source=./chirp.db");
        connection.Open();
    }
    
    // Could be in an utility class
    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
    
    public List<CheepViewModel> ReadCheeps(int page)
    {
        var command = connection.CreateCommand();

        command.CommandText =
            @"
                SELECT U.username, M.text, M.pub_date
                FROM message M
                JOIN user U ON M.author_id = U.user_id
                LIMIT 32 OFFSET @page * @PageLength
            ";
        
        command.Parameters.AddWithValue("@page", page);
        command.Parameters.AddWithValue("@PageLength", PageLength);
        
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var author = reader.GetString(0);
            var message = reader.GetString(1);
            var timestamp = reader.GetDouble(2);

            cheeps.Add(new CheepViewModel(author, message, UnixTimeStampToDateTimeString(timestamp)));
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
                LIMIT 32 OFFSET @page * @PageLength
            ";

        command.Parameters.AddWithValue("@Author", author);
        command.Parameters.AddWithValue("@page", page);
        command.Parameters.AddWithValue("@PageLength", PageLength);
        
        List<CheepViewModel> cheeps = new List<CheepViewModel>();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var message = reader.GetString(0);
            var timestamp = reader.GetDouble(1);

            cheeps.Add(new CheepViewModel(author,message, UnixTimeStampToDateTimeString(timestamp)));
        }

        return cheeps;
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
}