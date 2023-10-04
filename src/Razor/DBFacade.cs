using Microsoft.Data.Sqlite;
using Razor;

namespace SQLite;

public class DBFacade
{

    private static DBFacade? instance;

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
        connection = new SqliteConnection("Data Source=chirp.db");
        connection.Open();
    }

    public List<CheepViewModel> ReadCheeps()
    {
        var command = connection.CreateCommand();

        command.CommandText =
            @"
                SELECT U.username, M.text, M.pub_date
                FROM message M
                JOIN user U ON M.author_id = U.user_id
            ";
        
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
    
    // Could be in an utility class
    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}