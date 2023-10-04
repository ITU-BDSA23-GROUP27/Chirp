using SQLite;

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
        // filter by the provided author name
        //return DBFacade.Instance.ReadCheeps().Where(x => x.Author == author).ToList();

        return DBFacade.Instance.ReadCheepsFromAuthor(author, page);

        //throw new NotImplementedException();
    }

}