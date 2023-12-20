using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly ChirpContext _context;

    public UserRepository(ChirpContext context)
    {
        _context = context;
    }

    public async Task<UserDto> GetUserByName(string userName)
    {
        var user = await _context.Users.FirstAsync(u => u.Name == userName);

        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email ?? string.Empty,
        };
    }

    public async Task<UserDto> GetUserByEmail(string authorEmail)
    {
        var user = await _context.Users.FirstAsync(u => u.Email == authorEmail);

        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email ?? string.Empty,
        };
    }

    public async Task CreateUser(UserDto user)
    {
        var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Name == user.Name);

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
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(UserDto user)
    {
        var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Name == user.Name);
        
        if (existingUser is null)
        {
            throw new ArgumentException("Author does not exist: ", nameof(user));
        }
        
        _context.Users.Remove(existingUser);
        
        await _context.SaveChangesAsync();
    }
}