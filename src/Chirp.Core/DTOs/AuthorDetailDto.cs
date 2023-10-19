namespace Chirp.Core.DTOs;

public class AuthorDetailDto
{
    
    // made to public records instead ?
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public IEnumerable<Guid> CheepIds { get; set; } = new List<Guid>();
}