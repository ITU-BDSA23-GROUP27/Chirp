namespace Chirp.Core.DTOs;

public class CheepDto
{
    
    // made to public records instead ?
    public Guid Id { get; set; }
    public required string Message { get; set; }
    public required string TimeStamp { get; set; }
    public required string AuthorName { get; set; }
}