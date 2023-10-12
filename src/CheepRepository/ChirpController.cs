using System.Globalization;
using CheepRepository.Model;
using Microsoft.EntityFrameworkCore;

namespace CheepRepository;

public class ChirpController : IChirpController
{
    private ChirpDBContext _db;

    public ChirpController(ChirpDBContext db)
    {
        // If the database does not contain any cheeps or authors the sample data gets injected
        DbInitializer.SeedDatabase(db);
        _db = db;
    }
    
    public IQueryable<CheepDto> GetCheeps()
    {
        var cheeps = _db.Cheeps.Select<Cheep, CheepDto>(c => new CheepDto()
        {
            Id = c.CheepId,
            Message = c.Text,
            TimeStamp = c.TimeStamp.ToString(CultureInfo.InvariantCulture),
            AuthorName = c.Author.Name
        });
        return cheeps;
    }
    
    public AuthorDetailDto GetAuthor(Guid authorId)
    {
        var author = _db.Authors.Include(author => author.Cheeps).First(a => a.AuthorId == authorId);

        return new AuthorDetailDto()
        {
            Id = author.AuthorId,
            Name = author.Name,
            Email = author.Email,
            CheepIds = author.Cheeps.Select(c => c.CheepId)
        };
    }
    
    public IQueryable<AuthorDto> GetAuthors()
    {
        var authors = _db.Authors.Select<Author, AuthorDto>(a => new AuthorDto()
        {
            Id = a.AuthorId,
            Name = a.Name,
            Email = a.Email
        });
        return authors;
    }
}