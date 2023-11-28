using Chirp.Core.DTOs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Test
{
    public class UserRepositoryTest 
    {
        private readonly ChirpContext _context;
        private readonly CheepRepository _cheepRepository;
        private readonly UserRepository _userRepository;

        public UserRepositoryTest()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ChirpContext>()
                .UseSqlite(connection)
                .Options;

            _context = new ChirpContext(options);
            _context.Database.Migrate(); //TODO Get checked by TA

            _cheepRepository = new CheepRepository(_context);
            _userRepository = new UserRepository(_context);
        }

        [Fact]
        public async Task CreateUser_Success()
        {
            // Arrange
            var userDto = new UserDto { Name = "Bodil Bodilsen", Email = "Bodil@danmark.dk" };

            // Act
            await _userRepository.CreateUser(userDto);

            // Assert
            var user = _context.Users.Single(a => a.Name == "Bodil Bodilsen");
            Assert.Equal("Bodil Bodilsen", user.Name);
            Assert.Equal("Bodil@danmark.dk", user.Email);
        }
        
        [Fact]
        public async Task DeleteUser_Success()
        {
            // Arrange
            var user = new UserDto
            {
                Name = "DeletedUser", 
                Email = ""
            };
            
            await _userRepository.CreateUser(user);

            // Act
            await _userRepository.DeleteUser(user);

            // Assert
            Assert.DoesNotContain(_context.Users, u => u.Name == user.Name);
        }
        
        [Fact]
        public async Task GetUserByName_ReturnsCorrectUser()
        {
            // Arrange
            var user = new UserDto { Name = "Spongebob Squarepants", Email = "mrgoofy@pants.com" };
            await _userRepository.CreateUser(user);

            // Act
            var result = await _userRepository.GetUserByName("Spongebob Squarepants");

            // Assert
            Assert.Equal("Spongebob Squarepants", result.Name);
        }

        [Fact]
        public async Task GetUserByEmail_ReturnsCorrectUser()
        {
            // Arrange
            var user = new UserDto { Name = "Karsten Pedersen", Email = "kp67@email.com" };
            await _userRepository.CreateUser(user);

            // Act
            var result = await _userRepository.GetUserByEmail("kp67@email.com");

            // Assert
            Assert.Equal("Karsten Pedersen", result.Name);
        }
    }
}
