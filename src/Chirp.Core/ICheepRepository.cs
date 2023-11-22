using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface ICheepRepository
{
    public Task<IEnumerable<CheepDto>> GetCheeps();
    public Task<IEnumerable<CheepDto>> GetCheepsFromPage(int page);
    public Task<IEnumerable<CheepDto>> GetCheepsFromAuthor(string authorName);
    public Task<IEnumerable<CheepDto>> GetCheepsFromAuthorPage(string authorName, int page);
    public Task CreateCheep(CheepDto cheep);
}