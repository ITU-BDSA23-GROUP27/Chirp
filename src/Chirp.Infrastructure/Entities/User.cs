using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure.Entities;

/// <summary>
/// A representation of a User in the Chirp application.
/// </summary>

public class User : IdentityUser<Guid>
{
    public required string Name { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; } = new List<Cheep>();
}