using System.Globalization;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;

    private const int PageLimit = 32;

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
        return GetCheeps().Skip((page - 1) * PageLimit).Take(PageLimit);
    }
    
    public IEnumerable<CheepDto> GetCheepsFromAuthor(string authorName)
    {
        return GetCheeps().Where(c => c.AuthorName == authorName);
    }
    
    public IEnumerable<CheepDto> GetCheepsFromAuthorPage(string authorName, int page)
    {
        return GetCheepsFromAuthor(authorName).Skip((page - 1) * PageLimit).Take(PageLimit);
    }
    public void CreateCheep(CheepDto cheep)
    {
        var existingAuthor = _context.Authors.SingleOrDefault(c => c.Name == cheep.AuthorName);

        if (existingAuthor is null)
        {
            throw new ArgumentException("No existing author with that name found");
        }

        var newCheep = new Cheep()
        {
            CheepId = new Guid(),
            Text = cheep.Message,
            TimeStamp = DateTime.Parse(cheep.TimeStamp),
            Author = existingAuthor
            
        };
        
        _context.Cheeps.Add(newCheep);
        _context.SaveChanges();
    }
}