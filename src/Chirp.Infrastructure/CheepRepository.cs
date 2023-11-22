using System.Globalization;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpContext _context;

    private const int PageLimit = 32;

    public CheepRepository(ChirpContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<CheepDto>> GetCheeps()
    {
        var cheeps = await _context.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Select<Cheep, CheepDto>(c => new CheepDto()
        {
            Id = c.CheepId,
            Message = c.Text,
            TimeStamp = c.TimeStamp.ToString(CultureInfo.InvariantCulture),
            AuthorName = c.Author.Name
        }).ToListAsync();
        
        return cheeps;
    }
    
    public async Task<IEnumerable<CheepDto>> GetCheepsFromPage(int page)
    {
        var cheeps = await GetCheeps();
        return cheeps.Skip((page - 1) * PageLimit).Take(PageLimit);
    }
    
    public async Task<IEnumerable<CheepDto>> GetCheepsFromAuthor(string authorName)
    {
        var cheeps = await GetCheeps();
        return cheeps.Where(c => c.AuthorName == authorName);
    }
    
    public async Task<IEnumerable<CheepDto>> GetCheepsFromAuthorPage(string authorName, int page)
    {
        var cheepsFromAuthor = await GetCheepsFromAuthor(authorName);
        return cheepsFromAuthor.Skip((page - 1) * PageLimit).Take(PageLimit);
    }
    public async Task CreateCheep(CheepDto cheep)
    {
        var existingAuthor = await _context.Authors.SingleOrDefaultAsync(c => c.Name == cheep.AuthorName);

        if (existingAuthor is null)
        {
            throw new ArgumentException($"No existing author with that name found: {cheep.AuthorName}");
        }

        var newCheep = new Cheep()
        {
            CheepId = new Guid(),
            Text = cheep.Message,
            TimeStamp = DateTime.Parse(cheep.TimeStamp),
            Author = existingAuthor
            
        };
        
        _context.Cheeps.Add(newCheep);
        await _context.SaveChangesAsync();
    }
}