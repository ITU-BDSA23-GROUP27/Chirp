using SQLite;

namespace Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    public List<CheepViewModel> GetCheeps()
    {
        return DBFacade.Instance.ReadCheeps();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        //return DBFacade.Instance.ReadCheeps().Where(x => x.Author == author).ToList();

        return DBFacade.Instance.ReadCheepsFromAuthor(author);

        //throw new NotImplementedException();
    }

}