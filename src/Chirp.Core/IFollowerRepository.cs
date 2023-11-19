using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IFollowerRepository
{
    public IEnumerable<AuthorDto> GetFollowersFromAuthor(string authorName);
    public IEnumerable<AuthorDto> GetFolloweesFromAuthor(string authorName);
    public void AddOrRemoveFollower(string authorName, string followerName);
}