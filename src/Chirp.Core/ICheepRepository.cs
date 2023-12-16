using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface ICheepRepository
{
    public Task<IEnumerable<CheepDto>> GetCheeps();
    public Task<IEnumerable<CheepDto>> GetCheepsFromPage(int page);
    public Task<IEnumerable<CheepDto>> GetCheepsFromUser(string userName);
    public Task<IEnumerable<CheepDto>> GetCheepsFromUserPage(string userName, int page);
    public Task CreateCheep(CheepDto cheep);
}