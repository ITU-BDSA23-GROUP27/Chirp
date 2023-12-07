using Chirp.Core.DTOs;
using Chirp.Infrastructure.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Chirp.Infrastructure.Test;

public class ReactionRepositoryTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ChirpContext _context;
    private readonly ReactionRepository _reactionRepository;
    private readonly UserRepository _userRepository;
    private readonly CheepRepository _cheepRepository;
    
    public ReactionRepositoryTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ChirpContext>()
            .UseSqlite(connection)
            .Options;

        _context = new ChirpContext(options);
        _context.Database.Migrate(); // TODO Get checked by TA

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
        
        // Act - Add likes
        var foundCheep = await _context.Cheeps.SingleAsync(c => c.Text == cheep.Message && c.User.Name == cheep.UserName);
        
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
    public async Task HasUserLiked_ReturnsTrueIfUserHasLiked()
    {
        
    }
    
}