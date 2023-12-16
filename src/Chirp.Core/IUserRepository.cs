using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IUserRepository
{
    public Task<UserDto> GetUserByName(string authorName);
    public Task<UserDto> GetUserByEmail(string authorEmail);
    public Task CreateUser(UserDto user);
    public Task DeleteUser(UserDto user);
}