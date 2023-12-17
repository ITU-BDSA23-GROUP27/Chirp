namespace Chirp.Infrastructure.Entities;

/// <summary>
/// A representation a follower relationship between two users.
/// </summary>

public class Follower
{
    public Guid FollowerId { get; set; }
    public Guid FolloweeId { get; set; }
    public required User FollowerUser { get; set; }
    public required User FolloweeUser { get; set; }
}