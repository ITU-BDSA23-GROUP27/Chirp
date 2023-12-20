using Chirp.Core.DTOs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Chirp.Infrastructure.Test;

public class FollowerRepositoryTests 
{
    private readonly ChirpContext _context;
    private readonly FollowerRepository _followerRepository;
    private readonly UserRepository _userRepository;

    public FollowerRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ChirpContext>()
            .UseSqlite(connection)
            .Options;

            _context = new ChirpContext(options);
            _context.Database.Migrate();

        _followerRepository = new FollowerRepository(_context);
        _userRepository = new UserRepository(_context);
    }

    [Fact]
    public async Task GetFollowersFromUser_ReturnsFollowers()
    {
        // Arrange
        var user = new UserDto { Name = "Leonardo DiCaprio", Email = "Leo@hollywood.com" };
        await _userRepository.CreateUser(user);
        var follower = new UserDto { Name = "Tim Tim", Email = "timmy@roblox.dk" };
        await _userRepository.CreateUser(follower);
        await _followerRepository.AddOrRemoveFollower(user.Name, follower.Name);

        // Act
            
        var followersTask = await _followerRepository.GetFollowersFromUser(user.Name);
        var followers = followersTask;

        // Assert
        Assert.Contains(followers, f => f.Name == follower.Name);
    }
        
    [Fact]
    public async Task GetFolloweesFromUser_ReturnsFollowees()
    {
        // Arrange
        var follower = new UserDto { Name = "Charles Dick", Email = "charlie@evo.com" };
        await _userRepository.CreateUser(follower);
            
        var followee = new UserDto { Name = "Danny Fergurson", Email = "fergie@hotmail.com" };
        await _userRepository.CreateUser(followee);
            
        await _followerRepository.AddOrRemoveFollower(followee.Name, follower.Name);

        // Act
        var followeesTask = await _followerRepository.GetFolloweesFromUser(follower.Name);
        var followees = followeesTask;
        var followeesAslist = followees.ToList();

        // Assert
        Assert.Contains(followeesAslist, f => f.Name == followee.Name);
    }

    [Fact]
    public async Task AddFollower_Success()
    {
        // Arrange
        var user = new UserDto { Name = "Ethan Zinga", Email = "ethan@outlook.com" };
        await _userRepository.CreateUser(user);
            
        var follower = new UserDto { Name = "Fiona Shrek", Email = "fiona@royal.com" };
        await _userRepository.CreateUser(follower);

        // Act
        await _followerRepository.AddOrRemoveFollower(user.Name, follower.Name);

        // Assert
        var followRelationship = await _context.Followers.Include(f => f.FollowerUser)
            .Include(f => f.FolloweeUser).SingleAsync();
        Assert.Equal(follower.Name, followRelationship.FollowerUser.Name);
        Assert.Equal(user.Name, followRelationship.FolloweeUser.Name);
    }

    [Fact]
    public async Task RemoveFollower_Success()
    {
        // Arrange
        var user = new UserDto { Name = "Thomas The Train", Email = "ttt@test.com" };
        await _userRepository.CreateUser(user);
            
        var follower = new UserDto { Name = "Bob The Builder", Email = "btb@bricks.com" };
        await _userRepository.CreateUser(follower);
            
        await _followerRepository.AddOrRemoveFollower(user.Name, follower.Name); // Add first

        // Act
        await _followerRepository.AddOrRemoveFollower(user.Name, follower.Name); // Then remove

        // Assert
        Assert.DoesNotContain(_context.Followers, f => f.FollowerUser.Name == follower.Name && f.FolloweeUser.Name == user.Name);
    }

    [Fact]
    public async Task AddFollower_ThrowsException_IfUserDoesNotExist()
    {
        // Arrange
        var follower = new UserDto { Name = "Ian Malcolm", Email = "ian@ian.com" };
        await _userRepository.CreateUser(follower);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _followerRepository.AddOrRemoveFollower("NonExistentUser", follower.Name));
        Assert.Contains("User does not exist", exception.Message);
    }

    [Fact]
    public async Task AddFollower_ThrowsException_IfFollowerDoesNotExist()
    {
        // Arrange
        var user = new UserDto { Name = "Jane Gee", Email = "jane@yahoo.com" };
        await _userRepository.CreateUser(user);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _followerRepository.AddOrRemoveFollower(user.Name, "NonExistentFollower"));
        Assert.Contains("Follower does not exist", exception.Message);
    }
}