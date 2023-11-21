using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IAuthorRepository
{
    public UserDto GetUserByName(string authorName);
    public UserDto GetUserByEmail(string authorEmail);
    public void CreateUser(UserDto user);
}