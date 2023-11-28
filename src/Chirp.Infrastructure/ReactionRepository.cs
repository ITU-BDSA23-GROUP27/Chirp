using Chirp.Core;
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
    
    public async Task LikeCheep(Guid cheepId, Guid userId)
    {
        var cheep = await _context.Cheeps.SingleOrDefaultAsync(c => c.CheepId == cheepId);
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

        if (cheep is null || user is null)
        {
            throw new ArgumentException("Error: User or Cheep not found when adding reaction");
        }
        
        var newLike = new Reaction()
        {
            UserId = userId,
            CheepId = cheepId,
            User = user,
            Cheep = cheep,
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

    public async Task CommentOnCheep(Guid cheepId, Guid userId, string content)
    {
        var cheep = await _context.Cheeps.SingleOrDefaultAsync(c => c.CheepId == cheepId);
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);

        if (cheep is null || user is null)
        {
            throw new ArgumentException("Error: User or Cheep not found when adding reaction");
        }
        
        var newComment = new Reaction()
        {
            UserId = userId,
            CheepId = cheepId,
            User = user,
            Cheep = cheep,
            ReactionType = ReactionType.Comment,
            ReactionContent = content
        };

        _context.Reactions.Add(newComment);
        await _context.SaveChangesAsync();
    }
}