using Microsoft.Data.Sqlite;

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

    public List<T> ReadCheeps<T>()
    {
        var command = connection.CreateCommand();

        command.CommandText =
            @"
                SELECT U.username, M.text, M.pub_date
                FROM message M
                JOIN user U ON M.author_id = U.user_id
            ";
        
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var name = reader.GetString(0);

                Console.WriteLine($"Hello!");
            }
        }

        throw new NotImplementedException();
    }
}