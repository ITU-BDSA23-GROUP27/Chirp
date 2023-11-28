using Chirp.Core.DTOs;

namespace Chirp.Core;

public interface IReactionRepository
{
    public Task<int> GetLikeCount(Guid cheepId);
    public Task<IEnumerable<CommentDto>> GetCommentsFromCheep(Guid cheepId);
    public Task LikeCheep(Guid cheepId, Guid userId);
    public Task CommentOnCheep(CommentDto comment);
}