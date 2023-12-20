using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Chirp.Infrastructure.Test;

public class ReactionRepositoryTest
{
    private readonly ChirpContext _context;
    private readonly ReactionRepository _reactionRepository;
    private readonly UserRepository _userRepository;
    private readonly CheepRepository _cheepRepository;
    
    public ReactionRepositoryTest()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ChirpContext>()
            .UseSqlite(connection)
            .Options;

        _context = new ChirpContext(options);
        _context.Database.Migrate();

        _reactionRepository = new ReactionRepository(_context);
        _cheepRepository = new CheepRepository(_context);
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public async Task LikeCheep_SuccessOnLike()
    {
        // Arrange
        var user = new UserDto { Name = "Leo", Email = "leo@gmail.com"};
        await _userRepository.CreateUser(user);
    
        var cheep = new CheepDto { Message = "Hello World", UserName = user.Name, TimeStamp = DateTime.Now.ToString() };
        await _cheepRepository.CreateCheep(cheep);
        
        var foundCheep = await _context.Cheeps.SingleAsync(c => c.Text == cheep.Message && c.User.Name == cheep.UserName);
        var foundUser = await _context.Users.SingleAsync(u => u.Name == user.Name);
        
        // Act
        await _reactionRepository.LikeCheep(foundCheep.CheepId, user.Name);
        
        // Assert
        var like = await _context.Reactions.SingleOrDefaultAsync(r => 
            r.UserId == foundUser.Id && 
            r.CheepId == foundCheep.CheepId &&
            r.ReactionType == ReactionType.Like);
        
        Assert.NotNull(like);
        Assert.Equal(foundUser.Id, like.UserId);
        Assert.Equal(foundCheep.CheepId, like.CheepId);
    }

    [Fact]
    public async Task LikeCheep_SuccessOnUnlike()
    {
        // Arrange
        var user = new UserDto { Name = "Oleg", Email = "oleg@gmail.com"};
        await _userRepository.CreateUser(user);
    
        var cheep = new CheepDto { Message = "Hello World", UserName = user.Name, TimeStamp = DateTime.Now.ToString() };
        await _cheepRepository.CreateCheep(cheep);
        
        var foundCheep = await _context.Cheeps.SingleAsync(c => c.Text == cheep.Message && c.User.Name == cheep.UserName);
        var foundUser = await _context.Users.SingleAsync(u => u.Name == user.Name);
        
        // Act
        await _reactionRepository.LikeCheep(foundCheep.CheepId, user.Name); // Like
        await _reactionRepository.LikeCheep(foundCheep.CheepId, user.Name); // Then unlike
        
        // Assert
        var like = await _context.Reactions.SingleOrDefaultAsync(r => 
            r.UserId == foundUser.Id && 
            r.CheepId == foundCheep.CheepId &&
            r.ReactionType == ReactionType.Like);
        
        Assert.Null(like);
    }

    [Fact]
    public async Task GetLikeCount_ReturnsLikeCount()
    {
        // Arrange - Create users and cheep
        var user1 = new UserDto { Name = "Uno", Email = "uno@gmail.com"};
        await _userRepository.CreateUser(user1);
        var user2 = new UserDto { Name = "Dos", Email = "dos@gmail.com"};
        await _userRepository.CreateUser(user2);
    
        var cheep = new CheepDto { Message = "Hello World", UserName = user1.Name, TimeStamp = DateTime.Now.ToString() };
        await _cheepRepository.CreateCheep(cheep);
        
        var foundCheep = await _context.Cheeps.SingleAsync(c => c.Text == cheep.Message && c.User.Name == cheep.UserName);
        
        // Act - Add likes
        await _reactionRepository.LikeCheep(foundCheep.CheepId, user1.Name); // Like
        await _reactionRepository.LikeCheep(foundCheep.CheepId, user2.Name); // Then unlike
        
        // Assert
        var likeCount = await _reactionRepository.GetLikeCount(foundCheep.CheepId);
        Assert.Equal(2, likeCount);
    }
    
    [Fact]
    public async Task GetLikesFromCheep_ReturnsLikes()
    {
        // Arrange
        var user1 = new UserDto { Name = "One", Email = "one@gmail.com"};
        await _userRepository.CreateUser(user1);
        
        var user2 = new UserDto { Name = "Two", Email = "two@gmail.com"};
        await _userRepository.CreateUser(user2);
    
        var cheep = new CheepDto { Message = "Hello World", UserName = user1.Name, TimeStamp = DateTime.Now.ToString() };
        await _cheepRepository.CreateCheep(cheep);
        
        var foundCheep = await _context.Cheeps.SingleAsync(c => c.Text == cheep.Message && c.User.Name == cheep.UserName);
        var foundUser1 = await _context.Users.SingleAsync(u => u.Name == user1.Name);
        var foundUser2 = await _context.Users.SingleAsync(u => u.Name == user2.Name);
        
        // Act
        await _reactionRepository.LikeCheep(foundCheep.CheepId, user1.Name);
        await _reactionRepository.LikeCheep(foundCheep.CheepId, user2.Name);
        
        // Assert
        var likes = (await _reactionRepository.GetLikesFromCheep(foundCheep.CheepId)).ToList();
        Assert.Equal(2, likes.Count());
        Assert.Contains(likes, l => l.UserId == foundUser1.Id);
        Assert.Contains(likes, l => l.UserId == foundUser2.Id);
    }
    
    [Fact]
    public async Task HasUserLiked_ReturnsTrueOrFalse()
    {
        // Arrange
        var user1 = new UserDto { Name = "Cheep", Email = "cheep@gmail.com"};
        await _userRepository.CreateUser(user1);
        
        var user2 = new UserDto { Name = "Chirp", Email = "chirp@gmail.com"};
        await _userRepository.CreateUser(user2);
    
        var cheep = new CheepDto { Message = "Hello World", UserName = user1.Name, TimeStamp = DateTime.Now.ToString() };
        await _cheepRepository.CreateCheep(cheep);
        
        var foundCheep = await _context.Cheeps.SingleAsync(c => c.Text == cheep.Message && c.User.Name == cheep.UserName);
        var foundUser1 = await _context.Users.SingleAsync(u => u.Name == user1.Name);
        var foundUser2 = await _context.Users.SingleAsync(u => u.Name == user2.Name);
        
        // Act
        await _reactionRepository.LikeCheep(foundCheep.CheepId, user1.Name);
        
        // Assert
        var hasUser1Liked = await _reactionRepository.HasUserLiked(foundCheep.CheepId, user1.Name);
        var hasUser2Liked = await _reactionRepository.HasUserLiked(foundCheep.CheepId, user2.Name);
        
        Assert.True(hasUser1Liked);
        Assert.False(hasUser2Liked);
    }

    [Fact]
    public async Task CommentOnCheep_Success()
    {
        // Arrange
        var user = new UserDto { Name = "Cole", Email = "cole@gmail.com"};
        await _userRepository.CreateUser(user);
        
        var cheep = new CheepDto { Message = "Hello World", UserName = user.Name, TimeStamp = DateTime.Now.ToString() };
        await _cheepRepository.CreateCheep(cheep);
        
        var foundCheep = await _context.Cheeps.SingleAsync(c => c.Text == cheep.Message && c.User.Name == cheep.UserName);
        var foundUser = await _context.Users.SingleAsync(u => u.Name == user.Name);
        
        var comment = new ReactionDto
        {
            CheepId = foundCheep.CheepId,
            UserId = foundUser.Id,
            Comment = "This is a comment",
            TimeStamp = DateTime.Now.ToString()
        };
        
        // Act
        await _reactionRepository.CommentOnCheep(comment);
        
        // Assert
        var commentDb = await _context.Reactions.SingleAsync(r => 
            r.UserId == foundUser.Id && 
            r.CheepId == foundCheep.CheepId &&
            r.ReactionType == ReactionType.Comment &&
            r.ReactionContent == "This is a comment");

        Assert.NotNull(commentDb);
        Assert.Equal(comment.Comment, commentDb.ReactionContent);
    }
    
    [Fact]
    public async Task GetCommentsFromCheep_ReturnsComments()
    {
        // Arrange
        var user1 = new UserDto { Name = "Mikael", Email = "mikael@gmail.com"};
        await _userRepository.CreateUser(user1);
        var user2 = new UserDto { Name = "Klaus", Email = "klaus@gmail.com"};
        await _userRepository.CreateUser(user2);
        
        var cheep = new CheepDto { Message = "Hello World", UserName = user1.Name, TimeStamp = DateTime.Now.ToString() };
        await _cheepRepository.CreateCheep(cheep);
        
        var foundCheep = await _context.Cheeps.SingleAsync(c => c.Text == cheep.Message && c.User.Name == cheep.UserName);
        var foundUser1 = await _context.Users.SingleAsync(u => u.Name == user1.Name);
        var foundUser2 = await _context.Users.SingleAsync(u => u.Name == user2.Name);
        
        var comment1 = new ReactionDto
        {
            CheepId = foundCheep.CheepId,
            UserId = foundUser1.Id,
            Comment = "This is a comment",
            TimeStamp = DateTime.Now.ToString()
        };
        
        var comment2 = new ReactionDto
        {
            CheepId = foundCheep.CheepId,
            UserId = foundUser2.Id,
            Comment = "This is also a comment",
            TimeStamp = DateTime.Now.ToString()
        };
        
        // Act
        await _reactionRepository.CommentOnCheep(comment1);
        await _reactionRepository.CommentOnCheep(comment2);
        
        // Assert
        var first = _context.Reactions.SingleOrDefaultAsync(c =>
            c.UserId == foundUser1.Id && c.CheepId == foundCheep.CheepId && c.ReactionContent == comment1.Comment);
        var second = _context.Reactions.SingleOrDefaultAsync(c =>
            c.UserId == foundUser2.Id && c.CheepId == foundCheep.CheepId && c.ReactionContent == comment2.Comment);
        Assert.NotNull(first);
        Assert.NotNull(second);

        var comments = (await _reactionRepository.GetCommentsFromCheep(foundCheep.CheepId)).ToList();
        Assert.Equal(2, comments.Count);
        Assert.Contains(comments, c => c.UserId == foundUser1.Id && c.CheepId == foundCheep.CheepId && c.Comment == comment1.Comment);
        Assert.Contains(comments, c => c.UserId == foundUser2.Id && c.CheepId == foundCheep.CheepId && c.Comment == comment2.Comment);
    }
    
}