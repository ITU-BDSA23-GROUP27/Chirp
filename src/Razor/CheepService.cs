using CheepRepository;
using CheepRepository.Model;

namespace Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public IQueryable<CheepViewModel> GetCheeps(int page);
    public IQueryable<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    private readonly IChirpController _controller;
    public CheepService(IChirpController controller)
    {
        _controller = controller;
    }
    public IQueryable<CheepViewModel> GetCheeps(int page)
    {
        //return DBFacade.Instance.ReadCheeps(page);
        
        return _controller.GetCheeps().Select(c =>
            new CheepViewModel(c.AuthorName, c.Message, c.TimeStamp));
        
    }

    public IQueryable<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        //return DBFacade.Instance.ReadCheepsFromAuthor(author, page);
        return _controller.GetCheeps().Where(c => c.AuthorName == author)
            .Select(c => new CheepViewModel(c.AuthorName, c.Message, c.TimeStamp));
    }
}