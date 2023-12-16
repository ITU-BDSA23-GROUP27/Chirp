using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IFollowerRepository
{
    public Task<IEnumerable<UserDto>> GetFollowersFromUser(string userName);
    public Task<IEnumerable<UserDto>> GetFolloweesFromUser(string userName);
    public Task AddOrRemoveFollower(string userName, string followerName);
}