using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IAuthorRepository
{
    public AuthorDto GetAuthorByName(string authorName);
    public AuthorDto GetAuthorByEmail(string authorEmail);
    public void CreateAuthor(AuthorDto author);
}