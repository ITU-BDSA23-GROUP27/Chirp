using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IFollowerRepository
{
    public Task<IEnumerable<AuthorDto>> GetFollowersFromUser(string userName);
    public Task<IEnumerable<AuthorDto>> GetFolloweesFromUser(string userName);
    public Task AddOrRemoveFollower(string userName, string followerName);
}