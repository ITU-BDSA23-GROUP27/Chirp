namespace Chirp.Infrastructure.Entities;

public class Reaction
{
    public Guid UserId { get; set; }
    public Guid CheepId { get; set; }
    public required User User { get; set; }
    public required Cheep Cheep { get; set; }
    
    public required EnumReaction EnumReaction { get; set; }
}

public enum EnumReaction
{
    Like,
    Comment,
}