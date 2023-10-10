using CheepRepository.Model;

namespace CheepRepository;

public interface IChirpController
{
    public IQueryable<CheepDto> GetCheeps();
    public AuthorDetailDto GetAuthor(int authorId);
    public IQueryable<AuthorDto> GetAuthors();
}