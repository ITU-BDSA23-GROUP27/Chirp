namespace Chirp.Core.DTOs;

public class AuthorDetailDto
{
    
    // made to public records instead ?
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public IEnumerable<Guid> CheepIds { get; set; } = new List<Guid>();
}