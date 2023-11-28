using System.Globalization;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ReactionRepository : IReactionRepository
{
    private readonly ChirpContext _context;

    public ReactionRepository(ChirpContext context)
    {
        _context = context;
    }

    public async Task<int> GetLikeCount(Guid cheepId)
    {
        var likes = await _context.Reactions
            .Select(r => r.CheepId == cheepId && r.ReactionType == ReactionType.Like)
            .ToListAsync();

        return likes.Count;
    }

    public async Task<IEnumerable<CommentDto>> GetCommentsFromCheep(Guid cheepId)
    {
        var comments = await _context.Reactions
            .OrderByDescending(r => r.TimeStamp)
            .Select<Reaction, CommentDto>(r => new CommentDto()
                {
                    UserId = r.CheepId,
                    CheepId = r.CheepId,
                    TimeStamp = r.TimeStamp.ToString(CultureInfo.InvariantCulture),
                    Comment = r.ReactionContent
                }
            ).Where(r => r.CheepId == cheepId).ToListAsync();

        return comments;
    }
    
    public async Task LikeCheep(Guid cheepId, string userName)
    {
        var cheep = await _context.Cheeps.SingleOrDefaultAsync(c => c.CheepId == cheepId);
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Name == userName);

        if (cheep is null || user is null)
        {
            throw new ArgumentException("Error: User or Cheep not found when adding reaction");
        }
        
        var newLike = new Reaction()
        {
            UserId = user.Id,
            CheepId = cheepId,
            User = user,
            Cheep = cheep,
            TimeStamp = DateTime.UtcNow.AddHours(1),
            ReactionType = ReactionType.Like,
        };

        var existingLike = await _context.Reactions
            .SingleOrDefaultAsync(r => r.UserId == newLike.UserId 
                                       && r.CheepId == newLike.CheepId 
                                       && r.ReactionType == newLike.ReactionType
                                       && r.ReactionContent == newLike.ReactionContent);

        if (existingLike is null)
        {
            _context.Reactions.Add(newLike);
        }
        else
        {
            _context.Reactions.Remove(newLike);
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task CommentOnCheep(CommentDto comment)
    {
        var cheep = await _context.Cheeps.SingleOrDefaultAsync(c => c.CheepId == comment.CheepId);
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == comment.UserId);

        if (cheep is null || user is null)
        {
            throw new ArgumentException("Error: User or Cheep not found when adding reaction");
        }
        
        var newComment = new Reaction()
        {
            UserId = comment.UserId,
            CheepId = comment.CheepId,
            User = user,
            Cheep = cheep,
            TimeStamp = DateTime.Parse(comment.TimeStamp),
            ReactionType = ReactionType.Comment,
            ReactionContent = comment.Comment
        };

        _context.Reactions.Add(newComment);
        await _context.SaveChangesAsync();
    }
}