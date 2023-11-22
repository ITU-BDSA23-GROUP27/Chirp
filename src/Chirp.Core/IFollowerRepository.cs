using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IFollowerRepository
{
    public Task<IEnumerable<AuthorDto>> GetFollowersFromAuthor(string authorName);
    public Task<IEnumerable<AuthorDto>> GetFolloweesFromAuthor(string authorName);
    public Task AddOrRemoveFollower(string authorName, string followerName);
}