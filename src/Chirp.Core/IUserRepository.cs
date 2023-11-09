using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IUserRepository
{
    public UserDto GetUserByName(string userName);
    public UserDto GetUserByEmail(string userEmail);
    public void CreateUser(UserDto user);
}