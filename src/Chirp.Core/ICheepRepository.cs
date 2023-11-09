using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface ICheepRepository
{
    public IEnumerable<CheepDto> GetCheeps();
    public IEnumerable<CheepDto> GetCheepsFromPage(int page);
    public IEnumerable<CheepDto> GetCheepsFromUser(string userName);
    public IEnumerable<CheepDto> GetCheepsFromUserPage(string userName, int page);
    public void CreateCheep(CheepDto cheep);
}