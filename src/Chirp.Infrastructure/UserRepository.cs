using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

/// <summary>
/// Repository for handling Users in the Chirp application.
/// </summary>

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
            Email = user?.Email ?? string.Empty,
        };
    }

    public async Task<UserDto> GetUserByEmail(string authorEmail)
    {
        var user = await _context.Users.FirstAsync(u => u.Email == authorEmail);

        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user?.Email ?? string.Empty,
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
    
    /// <summary>
    /// Method for deleting all of the User's data including cheeps, reactions, followers and followees from the database.
    /// Since SQL server does not cascade delete on many-to-many relationships, we have to manually delete the entities
    /// </summary>
    /// <param name="user"></param>
    /// <exception cref="ArgumentException"></exception>

    public async Task DeleteUser(UserDto user)
    {
        var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Name == user.Name);
        
        if (existingUser is null)
        {
            throw new ArgumentException("User does not exist: ", nameof(user));
        }
        
        // Delete user's followees
        var deleteFollowees = await _context.Followers
            .Where(f => f.FolloweeId == existingUser.Id || f.FolloweeId == existingUser.Id)
            .ToListAsync();
        
        // Delete user's followers
        var deleteFollowers = await _context.Followers
            .Where(f => f.FollowerId == existingUser.Id || f.FollowerId == existingUser.Id)
            .ToListAsync();
        
        // Delete user id in reactions
        var deleteReactionUserId = await _context.Reactions
            .Where(r => r.UserId == existingUser.Id)
            .ToListAsync();
        
        // Delete user of type User in reactions
        var deleteReactionUser = await _context.Reactions
            .Where(r => r.Cheep.User == existingUser)
            .ToListAsync();
        
        // Delete user's cheeps
        var deleteUserCheeps = await _context.Cheeps
            .Where(c => c.User == existingUser)
            .ToListAsync();
        
        _context.Followers.RemoveRange(deleteFollowees);
        _context.Followers.RemoveRange(deleteFollowers);
        _context.Reactions.RemoveRange(deleteReactionUserId);
        _context.Reactions.RemoveRange(deleteReactionUser);
        _context.Cheeps.RemoveRange(deleteUserCheeps);
        
        _context.Users.Remove(existingUser);
        
        await _context.SaveChangesAsync();
    }
}