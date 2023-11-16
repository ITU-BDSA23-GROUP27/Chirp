namespace Chirp.Core;

public interface IFollowerRepository
{
    public void AddFollower(string authorName, string followerName);
}