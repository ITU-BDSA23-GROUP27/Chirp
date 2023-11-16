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
    public IEnumerable<AuthorDto> GetFollowersFromAuthor(int page, string authorName)
    {
        var followers = _context.Followers
            .Where(f => f.FollowerAuthor.Name == authorName)
            .Select(f => f.FolloweeAuthor)
            .Select<Author, AuthorDto>(a => new AuthorDto()
            {
                Id = a.AuthorId,
                Name = a.Name,
                Email = a.Email
            });

        return followers;
    }
    
    //Followees are the ones the author/user follows
    public IEnumerable<AuthorDto> GetFolloweesFromAuthor(int page, string authorName)
    {
        var followees = _context.Followers
            .Where(f => f.FolloweeAuthor.Name == authorName)
            .Select(f => f.FollowerAuthor)
            .Select<Author, AuthorDto>(a => new AuthorDto()
            {
                Id = a.AuthorId,
                Name = a.Name,
                Email = a.Email
            });

        return followees;
    }
    
    
    public void AddFollower(string authorName, string followerName)
    {
        var author = _context.Authors.SingleOrDefault(a => a.Name == authorName);
        var follower = _context.Authors.SingleOrDefault(a => a.Name == followerName);
        
        if (author == follower)
        {
            throw new ArgumentException("Author and follower cannot be equal to one another: ", nameof(authorName));
        }
        
        if (author is null)
        {
            throw new ArgumentException("Author does not exist: ", nameof(authorName));
        }
        
        if (follower is null)
        {
            throw new ArgumentException("Follower does not exist: ", nameof(followerName));
        }

        var newFollower = new Follower()
        {
            FollowerId = follower.AuthorId,
            FolloweeId = author.AuthorId,
            FollowerAuthor = follower,
            FolloweeAuthor = author
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