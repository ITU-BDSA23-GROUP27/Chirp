namespace Chirp.Core.DTOs;

public class CheepDto
{
    
    // made to public records instead ?
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string TimeStamp { get; set; }
    public string AuthorName { get; set; }
}