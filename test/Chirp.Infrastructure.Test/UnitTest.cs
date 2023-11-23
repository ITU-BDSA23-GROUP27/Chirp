using Chirp.Core.DTOs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Test
{
    public class RepositoryTests : IDisposable
    {
        private readonly ChirpContext _context;
        private readonly CheepRepository _cheepRepository;
        private readonly UserRepository _userRepository;

        public RepositoryTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ChirpContext>()
                .UseSqlite(connection)
                .Options;

            _context = new ChirpContext(options);
            //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

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
        public async Task CreateCheep_Success()
        {
            // Arrange
            var userDto = new UserDto { Name = "Hans Hansen", Email = "HH@outlook.com" };
            await _userRepository.CreateUser(userDto);
            var cheepDto = new CheepDto
            {
                Message = "Bye, world!",
                TimeStamp = DateTime.UtcNow.ToString(),
                UserName = "Hans Hansen"
            };

            // Act
            await _cheepRepository.CreateCheep(cheepDto);

            // Assert
            var cheep = _context.Cheeps.Include(cheep => cheep.User).Single();
            Assert.Equal("Bye, world!", cheep.Text);
            Assert.Equal("Hans Hansen", cheep.User.Name);
        }

        [Fact]
        public async Task GetCheeps_ReturnsAllCheeps()
        {
            // Arrange
            var user = new UserDto { Name = "Omar Semou", Email = "OmarSemou@hotmail.com" };
            await _userRepository.CreateUser(user);
            var cheep = new CheepDto { UserName = "Omar Semou", Message = "Testing 1 2 3", TimeStamp = DateTime.UtcNow.ToString() };
            await _cheepRepository.CreateCheep(cheep);

            // Act
            var result = await _cheepRepository.GetCheeps();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task GetCheepsFromPage_ReturnsCorrectPage()
        {
            // Arrange
            var user = new UserDto { Name = "Darryl Davidson", Email = "merica4lyfe@usa.com" };
            await _userRepository.CreateUser(user);
            for (int i = 0; i < 50; i++)
            {
                var cheep = new CheepDto { UserName = "Darryl Davidson", Message = $"I ain't afriad {i}", TimeStamp = DateTime.UtcNow.ToString() };
                await _cheepRepository.CreateCheep(cheep);
            }

            // Act
            var result = await _cheepRepository.GetCheepsFromPage(2);

            // Assert
            Assert.Equal(18, result.Count()); 
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

        public void Dispose()
        {
            //_context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
