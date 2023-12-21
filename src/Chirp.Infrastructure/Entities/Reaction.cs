namespace Chirp.Infrastructure.Entities;

/// <summary>
/// A Reaction is a representation of a reaction from a User to a Cheep e.g. likes and comments.
/// </summary>
public class Reaction
{
    public Guid UserId { get; set; }
    public Guid CheepId { get; set; }
    public required User User { get; set; }
    public required Cheep Cheep { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required ReactionType ReactionType { get; set; }
    public string? ReactionContent { get; set; }
}

public enum ReactionType
{
    Like,
    Comment
}