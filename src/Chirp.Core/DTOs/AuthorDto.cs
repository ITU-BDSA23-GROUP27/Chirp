namespace Chirp.Core.DTOs;

public class AuthorDto
{
    // made to public records instead ?
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}