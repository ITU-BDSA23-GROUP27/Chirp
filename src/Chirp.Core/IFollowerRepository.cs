using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IFollowerRepository
{
    public IEnumerable<UserDto> GetFollowersFromUser(string userName);
    public IEnumerable<UserDto> GetFolloweesFromUser(string userName);
    public void AddOrRemoveFollower(string userName, string followerName);
}