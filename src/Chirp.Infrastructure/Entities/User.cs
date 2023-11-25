using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure.Entities;

public class User : IdentityUser<Guid>
{
    public required string Name { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; } = new List<Cheep>();
    public bool IsDeleted { get; set; }
}