namespace Chirp.Core.DTOs;

public class UserDto
{
    // made to public records instead ?
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}