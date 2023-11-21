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
            UserName = c.User.Name
        });
        return cheeps;
    }
    
    public IEnumerable<CheepDto> GetCheepsFromPage(int page)
    {
        return GetCheeps().Skip((page - 1) * PageLimit).Take(PageLimit);
    }
    
    public IEnumerable<CheepDto> GetCheepsFromUser(string userName)
    {
        return GetCheeps().Where(c => c.UserName == userName);
    }
    
    public IEnumerable<CheepDto> GetCheepsFromUserPage(string userName, int page)
    {
        return GetCheepsFromUser(userName).Skip((page - 1) * PageLimit).Take(PageLimit);
    }
    public void CreateCheep(CheepDto cheep)
    {
        var existingUser = _context.Users.SingleOrDefault(u => u.Name == cheep.UserName);

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
        _context.SaveChanges();
    }
}