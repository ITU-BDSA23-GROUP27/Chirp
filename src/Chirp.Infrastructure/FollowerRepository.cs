using System.Collections;
using System.Globalization;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;

namespace Chirp.Infrastructure;

public class FollowerRepository : IFollowerRepository
{
    private readonly ChirpContext _context;
    public FollowerRepository(ChirpContext context)
    {
        _context = context;
    }
    
    //Followers are the ones that follows the author/user
    public IEnumerable<UserDto> GetFollowersFromUser(string userName)
    {
        var followers = _context.Followers
            .Where(f => f.FolloweeUser.Name == userName)
            .Select(f => f.FollowerUser)
            .Select<User, UserDto>(f => new UserDto()
            {
                Id = f.Id,
                Name = f.Name,
                Email = f.Email
            });

        return followers;
    }
    
    //Followees are the ones the author/user follows
    public IEnumerable<UserDto> GetFolloweesFromUser(string userName)
    {
        var followees = _context.Followers
            .Where(f => f.FollowerUser.Name == userName)
            .Select(f => f.FolloweeUser)
            .Select<User, UserDto>(f => new UserDto()
            {
                Id = f.Id,
                Name = f.Name,
                Email = f.Email
            });

        return followees;
    }
    
    public void AddOrRemoveFollower(string userName, string followerName)
    {
        var user = _context.Users.SingleOrDefault(a => a.Name == userName);
        var follower = _context.Users.SingleOrDefault(a => a.Name == followerName);
        
        if (user == follower)
        {
            throw new ArgumentException("Author and follower cannot be equal to one another: ", nameof(userName));
        }
        
        if (user is null)
        {
            throw new ArgumentException("Author does not exist: ", nameof(userName));
        }
        
        if (follower is null)
        {
            throw new ArgumentException("Follower does not exist: ", nameof(followerName));
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
        _context.SaveChanges();
    }
}