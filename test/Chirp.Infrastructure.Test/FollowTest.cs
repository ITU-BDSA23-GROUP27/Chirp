using Chirp.Core.DTOs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using System.Linq;


namespace Chirp.Infrastructure.Test
{
    public class FollowerRepositoryTests 
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ChirpContext _context;
        private readonly FollowerRepository _followerRepository;
        private readonly AuthorRepository _authorRepository;

        public FollowerRepositoryTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ChirpContext>()
                .UseSqlite(connection)
                .Options;

            _context = new ChirpContext(options);
            _context.Database.Migrate(); // TODO Get checked by TA

            _followerRepository = new FollowerRepository(_context);
            _authorRepository = new AuthorRepository(_context);
        }

        [Fact]
        public async Task GetFollowersFromAuthor_ReturnsFollowers()
        {
            // Arrange
            var author = new AuthorDto { Name = "Leonardo DiCaprio", Email = "Leo@hollywood.com" };
            await _authorRepository.CreateAuthor(author);
            var follower = new AuthorDto { Name = "Tim Tim", Email = "timmy@roblox.dk" };
            await _authorRepository.CreateAuthor(follower);
            await _followerRepository.AddOrRemoveFollower(author.Name, follower.Name);

            // Act
            //var followers = _followerRepository.GetFollowersFromAuthor(author.Name);
            
            var followersTask = _followerRepository.GetFollowersFromAuthor(author.Name);
            var followers = await followersTask;

            // Assert
            //Assert.Contains(followers, f => f.Name == follower.Name);
            Assert.Contains<AuthorDto>(followers, f => f.Name == follower.Name);
        }
        
        [Fact]
        public async Task GetFolloweesFromAuthor_ReturnsFollowees()
        {
            // Arrange
            var follower = new AuthorDto { Name = "Charles Dick", Email = "charlie@evo.com" };
            await _authorRepository.CreateAuthor(follower);
            var followee = new AuthorDto { Name = "Danny Fergurson", Email = "fergie@hotmail.com" };
            await _authorRepository.CreateAuthor(followee);
            await _followerRepository.AddOrRemoveFollower(followee.Name, follower.Name);

            // Act
            var followeesTask = _followerRepository.GetFolloweesFromAuthor(follower.Name);
            var followees = await followeesTask;
            var followeesAslist = followees.ToList();

            // Assert
            Assert.Contains(followeesAslist, f => f.Name == followee.Name);
        }

        [Fact]
        public async Task AddFollower_Success()
        {
            // Arrange
            var author = new AuthorDto { Name = "Ethan Zinga", Email = "ethan@outlook.com" };
            await _authorRepository.CreateAuthor(author);
            var follower = new AuthorDto { Name = "Fiona Shrek", Email = "fiona@royal.com" };
            await _authorRepository.CreateAuthor(follower);

            // Act
            await _followerRepository.AddOrRemoveFollower(author.Name, follower.Name);

            // Assert
            var followRelationship = _context.Followers.Single();
            Assert.Equal(follower.Name, followRelationship.FollowerAuthor.Name);
            Assert.Equal(author.Name, followRelationship.FolloweeAuthor.Name);
        }

        [Fact]
        public async Task RemoveFollower_Success()
        {
            // Arrange
            var author = new AuthorDto { Name = "Thomas The Train", Email = "ttt@test.com" };
            await _authorRepository.CreateAuthor(author);
            var follower = new AuthorDto { Name = "Bob The Builder", Email = "btb@bricks.com" };
            await _authorRepository.CreateAuthor(follower);
            await _followerRepository.AddOrRemoveFollower(author.Name, follower.Name); // Add first

            // Act
            await _followerRepository.AddOrRemoveFollower(author.Name, follower.Name); // Then remove

            // Assert
            Assert.DoesNotContain(_context.Followers, f => f.FollowerAuthor.Name == follower.Name && f.FolloweeAuthor.Name == author.Name);
        }

        [Fact]
        public async Task AddFollower_ThrowsException_IfAuthorDoesNotExist()
        {
            // Arrange
            var follower = new AuthorDto { Name = "Ian Malcolm", Email = "ian@ian.com" };
            await _authorRepository.CreateAuthor(follower);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _followerRepository.AddOrRemoveFollower("NonExistentAuthor", follower.Name));
            Assert.Contains("Author does not exist", exception.Message);
        }

        [Fact]
        public async Task AddFollower_ThrowsException_IfFollowerDoesNotExist()
        {
            // Arrange
            var author = new AuthorDto { Name = "Jane Gee", Email = "jane@yahoo.com" };
            await _authorRepository.CreateAuthor(author);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _followerRepository.AddOrRemoveFollower(author.Name, "NonExistentFollower"));
            Assert.Contains("Follower does not exist", exception.Message);
        }
    }
}
