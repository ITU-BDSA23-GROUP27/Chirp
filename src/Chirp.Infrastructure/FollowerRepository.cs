using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class FollowerRepository : IFollowerRepository
{
    private readonly ChirpContext _context;
    public FollowerRepository(ChirpContext context)
    {
        _context = context;
    }
    
    //Followers are the ones that follows the author/user
    public async Task<IEnumerable<UserDto>> GetFollowersFromUser(string userName)
    {
        var followers = await _context.Followers
            .Where(f => f.FolloweeUser.Name == userName)
            .Select(f => f.FollowerUser)
            .Select<User, UserDto>(f => new UserDto()
            {
                Id = f.Id,
                Name = f.Name,
                Email = f.Email?? string.Empty
            }).ToListAsync();

        return followers;
    }
    
    //Followees are the ones the author/user follows
    public async Task<IEnumerable<UserDto>> GetFolloweesFromUser(string userName)
    {
        var followees = await _context.Followers
            .Where(f => f.FollowerUser.Name == userName)
            .Select(f => f.FolloweeUser)
            .Select<User, UserDto>(f => new UserDto()
            {
                Id = f.Id,
                Name = f.Name,
                Email = f.Email?? string.Empty
            }).ToListAsync();

        return followees;
    }
    
    public async Task AddOrRemoveFollower(string userName, string followerName)
    {
        var user = await _context.Users.SingleOrDefaultAsync(a => a.Name == userName);
        var follower = _context.Users.SingleOrDefault(a => a.Name == followerName);
        
        if (user == follower)
        {
            throw new ArgumentException("User and follower cannot be equal to one another: ", nameof(userName));
        }
        
        if (user is null)
        {
            throw new ArgumentException("User does not exist: ", nameof(userName));
        }
        
        if (follower is null)
        {
            throw new ArgumentException("Follower does not exist: ", nameof(followerName));
        }
        
        var existingFollower = await _context.Followers.SingleOrDefaultAsync(f =>
            f.FolloweeUser.Name == userName && f.FollowerUser.Name == followerName);
        
        if (existingFollower != null)
        {
            _context.Followers.Remove(existingFollower);
        }

        var newFollower = new Follower()
        {
            FollowerId = follower.Id,
            FolloweeId = user.Id,
            FollowerUser = follower,
            FolloweeUser = user
        };
        
        if (_context.Followers.Contains(newFollower))
        {
            _context.Followers.Remove(newFollower);
        }
        else
        {
            _context.Followers.Add(newFollower);
        }
        await _context.SaveChangesAsync();
    }
}