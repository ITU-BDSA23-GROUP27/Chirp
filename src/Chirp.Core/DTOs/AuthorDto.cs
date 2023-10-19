namespace Chirp.Core.DTOs;

public class AuthorDto
{
    // made to public records instead ?
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}