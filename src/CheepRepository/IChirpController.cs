using CheepRepository.Model;

namespace CheepRepository;

public interface IChirpController
{
    public IQueryable<CheepDto> GetCheeps();
    public AuthorDetailDto GetAuthor(Guid authorId);
    public IQueryable<AuthorDto> GetAuthors();
}