namespace Chirp.CSVDBService.DataBaseHandling;

public interface IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null);
    public void Store(T record);
}