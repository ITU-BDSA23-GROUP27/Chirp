namespace Chirp.Core;

public interface IReactionRepository
{
    public Task LikeCheep(Guid cheepId, Guid userId);
    public Task CommentOnCheep(Guid cheepId, Guid userId, string content);
}