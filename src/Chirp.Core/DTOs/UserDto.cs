namespace Chirp.Core.DTOs;

/// <summary>
/// Data Transfer Object (DTO) for Users in the Chirp application.
/// </summary>

public class UserDto
{
    public Guid Id { get; set; }
    public required string Name { get; init; }
    public required string Email { get; init; }
}