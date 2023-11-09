using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;

namespace Chirp.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly ChirpContext _context;

    public UserRepository(ChirpContext context)
    {
        _context = context;
    }

    public UserDto GetUserByName(string userName)
    {
        var user = _context.Users.First(a => a.Name == userName);

        return new UserDto()
        {
            Id = user.UserId,
            Name = user.Name,
            Email = user.Email,
        };
    }

    public UserDto GetUserByEmail(string userEmail)
    {
        var user = _context.Users.First(a => a.Email == userEmail);

        return new UserDto()
        {
            Id = user.UserId,
            Name = user.Name,
            Email = user.Email,
        };
    }

    public void CreateUser(UserDto user)
    {
        var existingUser = _context.Users.SingleOrDefault(c => c.Name == user.Name);

        if (existingUser is not null)
        {
            throw new ArgumentException("User already exists: ", nameof(user));
        }

        var newUser = new User
        {
            UserId = new Guid(),
            Name = user.Name,
            Email = user.Email,
        };
        
        _context.Users.Add(newUser);
        _context.SaveChanges();
    }
}