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

    public async Task<AuthorDto> GetAuthorByName(string authorName)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == authorName);

        if (author is null)
        {
            throw new ArgumentException("Author doesn't exists: ", nameof(author));
        }
        
        return new AuthorDto()
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
        };
    }

    public async Task<AuthorDto> GetAuthorByEmail(string authorEmail)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Email == authorEmail);

        return new AuthorDto()
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
        };
    }

    public async Task CreateAuthor(AuthorDto author)
    {
        var existingAuthor = await _context.Authors.SingleOrDefaultAsync(c => c.Name == author.Name);

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
        await _context.SaveChangesAsync();
    }
}