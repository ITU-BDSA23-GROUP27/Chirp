namespace Chirp.CSVDBService.DataBaseHandling.Utility;

public abstract class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        get => instance ??= new T();
        private set => instance = value;
    }
}