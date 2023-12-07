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
        
        // Act
        await _reactionRepository.LikeCheep(cheep.Id, user.Name);
        
        // Assert
        var like = await _context.Reactions.SingleOrDefaultAsync(r => 
            r.UserId == user.Id && 
            r.CheepId == cheep.Id &&
            r.ReactionType == ReactionType.Like);
        
        Assert.NotNull(like);
        Assert.Equal(user.Id, like.UserId);
        Assert.Equal(cheep.Id, like.CheepId);
    }

    [Fact]
    public async Task LikeCheep_SuccessOnUnlike()
    {
        
    }

    [Fact]
    public async Task GetLikeCount_ReturnsLikeCount()
    {
        
    }
    
}