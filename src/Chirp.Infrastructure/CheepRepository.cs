using System.Globalization;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

/// <summary>
/// Repository for handling Cheeps in the Chirp application.
/// A Cheep is a representation of a post in the Chirp application.
/// Cheeps are used for users to post messages and to display messages on the timelines.
/// </summary>

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
            UserName = c.User.Name
        }).ToListAsync();
        
        return cheeps;
    }
    
    public async Task<IEnumerable<CheepDto>> GetCheepsFromPage(int page)
    {
        var cheeps = await GetCheeps();
        return cheeps.Skip((page - 1) * PageLimit).Take(PageLimit);
    }
    
    public async Task<IEnumerable<CheepDto>> GetCheepsFromUser(string userName)
    {
        var cheeps = await GetCheeps();
        return cheeps.Where(c => c.UserName == userName);
    }
    
    public async Task<IEnumerable<CheepDto>> GetCheepsFromUserPage(string userName, int page)
    {
        var cheepsFromUser = await GetCheepsFromUser(userName);
        return cheepsFromUser.Skip((page - 1) * PageLimit).Take(PageLimit);
    }
    
    public async Task CreateCheep(CheepDto cheep)
    {
        var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Name == cheep.UserName);

        if (existingUser is null)
        {
            throw new ArgumentException($"No existing author with that name found: {cheep.UserName}");
        }

        var newCheep = new Cheep()
        {
            CheepId = new Guid(),
            Text = cheep.Message,
            TimeStamp = DateTime.Parse(cheep.TimeStamp),
            User = existingUser
            
        };
        
        _context.Cheeps.Add(newCheep);
        await _context.SaveChangesAsync();
    }
}