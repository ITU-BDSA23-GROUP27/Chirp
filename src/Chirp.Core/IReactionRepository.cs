using Chirp.Core.DTOs;

namespace Chirp.Core;

/// <summary>
/// Interface of the repository for handling reactions to Cheeps from Users e.g. likes and comments.
/// </summary>

public interface IReactionRepository
{
    public Task<int> GetLikeCount(Guid cheepId);
    public Task<bool> HasUserLiked(Guid cheepId, string userName);
    public Task<IEnumerable<ReactionDto>> GetCommentsFromCheep(Guid cheepId);
    public Task LikeCheep(Guid cheepId, string userName);
    public Task CommentOnCheep(ReactionDto comment);
}