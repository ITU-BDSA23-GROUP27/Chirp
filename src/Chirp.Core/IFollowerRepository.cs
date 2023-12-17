using Chirp.Core.DTOs;

namespace Chirp.Core;

/// <summary>
/// Interface of the repository for handling both followers and followees in the Chirp application.
/// Followers are the ones that follows the User
/// Followees are the ones the User follows
/// </summary>
/// 
public interface IFollowerRepository
{
    public Task<IEnumerable<UserDto>> GetFollowersFromUser(string userName);
    public Task<IEnumerable<UserDto>> GetFolloweesFromUser(string userName);
    public Task AddOrRemoveFollower(string userName, string followerName);
}