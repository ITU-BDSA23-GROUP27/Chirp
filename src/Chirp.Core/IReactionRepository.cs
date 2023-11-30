using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IReactionRepository
{
    public Task<int> GetLikeCount(Guid cheepId);
    public Task<IEnumerable<ReactionDto>> GetLikesFromCheep(Guid cheepId);
    public Task<IEnumerable<ReactionDto>> GetCommentsFromCheep(Guid cheepId);
    public Task LikeCheep(Guid cheepId, string userName);
    public Task CommentOnCheep(ReactionDto comment);
}