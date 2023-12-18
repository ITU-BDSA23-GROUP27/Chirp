using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chirp.Infrastructure.Entities;

/// <summary>
/// A Cheep is a representation of a post in the Chirp application.
/// Cheeps are used for users to post messages and to display messages on the timelines.
/// </summary>

public class Cheep
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CheepId { get; set; }
    
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public required User User { get; set; }
}