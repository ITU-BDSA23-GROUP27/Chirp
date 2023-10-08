namespace Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    public List<CheepViewModel> GetCheeps(int page)
    {
        return DBFacade.Instance.ReadCheeps(page);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        return DBFacade.Instance.ReadCheepsFromAuthor(author, page);
    }
}