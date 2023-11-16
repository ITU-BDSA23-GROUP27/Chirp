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

    public AuthorDto GetAuthorByName(string authorName)
    {
        var author = _context.Authors.First(a => a.Name == authorName);

        return new AuthorDto()
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
        };
    }

    public AuthorDto GetAuthorByEmail(string authorEmail)
    {
        var author = _context.Authors.First(a => a.Email == authorEmail);

        return new AuthorDto()
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
        };
    }

    public void CreateAuthor(AuthorDto author)
    {
        var existingAuthor = _context.Authors.SingleOrDefault(c => c.Name == author.Name);

        if (existingAuthor is not null)
        {
            throw new ArgumentException("Author already exists: ", nameof(author));
        }

        var newAuthor = new Author
        {
            AuthorId = new Guid(),
            Name = author.Name,
            Email = author.Email,
        };
        
        _context.Authors.Add(newAuthor);
        _context.SaveChanges();
    }
    
    public void AddFollower(string authorName, string followerName) //Should maybe be in own repository
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