using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly ChirpDbContext dbContext;

    public UserRepository(ChirpDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<UserDto> GetUserByName(string userName)
    {
        var user = await dbContext.Users.FirstAsync(u => u.Name == userName);

        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user?.Email ?? string.Empty,
        };
    }

    public async Task<UserDto> GetUserByEmail(string authorEmail)
    {
        var user = await dbContext.Users.FirstAsync(u => u.Email == authorEmail);

        return new UserDto()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user?.Email ?? string.Empty,
        };
    }

    public async Task CreateUser(UserDto user)
    {
        var existingUser = await dbContext.Users.SingleOrDefaultAsync(u => u.Name == user.Name);

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
        
        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteUser(UserDto user)
    {
        var existingUser = await dbContext.Users.SingleOrDefaultAsync(u => u.Name == user.Name);
        
        if (existingUser is null)
        {
            throw new ArgumentException("Author does not exist: ", nameof(user));
        }
        
        dbContext.Users.Remove(existingUser);
        
        await dbContext.SaveChangesAsync();
    }
}