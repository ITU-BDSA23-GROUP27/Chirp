using System.Globalization;
using Chirp.Core;
using Chirp.Core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private ChirpContext _context;

    private const int pageLimit = 32;

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }
    
    public IEnumerable<CheepDto> GetCheeps()
    {
        var cheeps = _context.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Select<Cheep, CheepDto>(c => new CheepDto()
        {
            Id = c.CheepId,
            Message = c.Text,
            TimeStamp = c.TimeStamp.ToString(CultureInfo.InvariantCulture),
            AuthorName = c.Author.Name
        });
        return cheeps;
    }
    
    public IEnumerable<CheepDto> GetCheepsFromPage(int page)
    {
        return GetCheeps().Skip((page - 1) * pageLimit).Take(pageLimit);
    }
    
    public IEnumerable<CheepDto> GetCheepsFromAuthor(string authorName, int page)
    {
        var cheeps = GetCheeps().Where(c => c.AuthorName == authorName);
        return cheeps.Skip((page - 1) * pageLimit).Take(pageLimit);
    }
    
    public AuthorDetailDto GetAuthor(Guid authorId)
    {
        var author = _context.Authors.Include(author => author.Cheeps).First(a => a.AuthorId == authorId);

        return new AuthorDetailDto()
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
            CheepIds = author.Cheeps.Select(c => c.CheepId)
        };
    }
    
    public IEnumerable<AuthorDto>GetAuthors()
    {
        var authors = _context.Authors.Select<Author, AuthorDto>(a => new AuthorDto()
        {
            Id = a.AuthorId,
            Name = a.Name,
            Email = a.Email
        });
        return authors;
    }
}