using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface ICheepRepository
{
    public IEnumerable<CheepDto> GetCheeps();
    public IEnumerable<CheepDto> GetCheepsFromPage(int page);
    public IEnumerable<CheepDto> GetCheepsFromAuthor(string authorName, int page);
    public AuthorDetailDto GetAuthor(Guid authorId);
    public IEnumerable<AuthorDto> GetAuthors();
}