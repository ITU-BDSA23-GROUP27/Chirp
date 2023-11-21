using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chirp.Infrastructure.Entities;

public class Cheep
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid CheepId { get; set; }
    
    public required string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public required User User { get; set; }
}