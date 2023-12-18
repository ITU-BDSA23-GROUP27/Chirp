using Chirp.Core.DTOs;

namespace Chirp.Core;

/// <summary>
/// Interface of the repository for handling Cheeps in the Chirp application.
/// A Cheep is a representation of a post in the Chirp application.
/// Cheeps are used for users to post messages and to display messages on the timelines.
/// </summary>

public interface ICheepRepository
{
    public Task<IEnumerable<CheepDto>> GetCheeps();
    public Task<IEnumerable<CheepDto>> GetCheepsFromPage(int page);
    public Task<IEnumerable<CheepDto>> GetCheepsFromUser(string userName);
    public Task CreateCheep(CheepDto cheep);
}