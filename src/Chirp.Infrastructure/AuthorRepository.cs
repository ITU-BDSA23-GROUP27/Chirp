using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;

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
}