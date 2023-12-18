using Chirp.Core.DTOs;

namespace Chirp.Core;

/// <summary>
/// Interface of the repository for handling Users in the Chirp application.
/// </summary>

public interface IUserRepository
{
    public Task<UserDto> GetUserByName(string authorName);
    public Task CreateUser(UserDto user);
    public Task DeleteUser(UserDto user);
}