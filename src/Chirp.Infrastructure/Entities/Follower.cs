namespace Chirp.Infrastructure.Entities;

public class Follower
{
    public Guid FollowerId { get; set; }
    public Guid FolloweeId { get; set; }
    public required Author FollowerAuthor { get; set; }
    public required Author FolloweeAuthor { get; set; }
}