using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IAuthorRepository
{
    public Task<AuthorDto> GetAuthorByName(string authorName);
    public Task<AuthorDto> GetAuthorByEmail(string authorEmail);
    public Task CreateAuthor(AuthorDto author);
}