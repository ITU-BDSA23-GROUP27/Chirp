using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpContext _context;

    public AuthorRepository(ChirpContext context)
    {
        _context = context;
    }

    public UserDto GetUserByName(string authorName)
    {
        var user = _context.Users.First(u => u.Name == authorName);

        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
        };
    }

    public UserDto GetUserByEmail(string authorEmail)
    {
        var user = _context.Users.First(u => u.Email == authorEmail);

        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
        };
    }

    public void CreateUser(UserDto user)
    {
        var existingUser = _context.Users.SingleOrDefault(u => u.Name == user.Name);

        if (existingUser is not null)
        {
            throw new ArgumentException("Author already exists: ", nameof(user));
        }

        var newUser = new User
        {
            Id = new Guid(),
            Name = user.Name,
            Email = user.Email,
        };
        
        _context.Users.Add(newUser);
        _context.SaveChanges();
    }
}