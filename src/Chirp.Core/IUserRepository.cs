using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IUserRepository
{
    public UserDto GetUserByName(string authorName);
    public UserDto GetUserByEmail(string authorEmail);
    public void CreateUser(UserDto user);
}