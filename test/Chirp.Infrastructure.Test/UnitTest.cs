using Chirp.Core.DTOs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Test
{
    public class RepositoryTests : IDisposable
    {
        private readonly ChirpContext _context;
        private readonly CheepRepository _cheepRepository;
        private readonly AuthorRepository _authorRepository;

        public RepositoryTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ChirpContext>()
                .UseSqlite(connection)
                .Options;

            _context = new ChirpContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _cheepRepository = new CheepRepository(_context);
            _authorRepository = new AuthorRepository(_context);
        }

        [Fact]
        public void CreateAuthor_Success()
        {
            // Arrange
            var authorDto = new AuthorDto { Name = "Bodil Bodilsen", Email = "Bodil@danmark.dk" };

            // Act
            _authorRepository.CreateAuthor(authorDto);

            // Assert
            var author = _context.Authors.Single(a => a.Name == "Bodil Bodilsen");
            Assert.Equal("Bodil Bodilsen", author.Name);
            Assert.Equal("Bodil@danmark.dk", author.Email);
        }

        [Fact]
        public void CreateCheep_Success()
        {
            // Arrange
            var authorDto = new AuthorDto { Name = "Hans Hansen", Email = "HH@outlook.com" };
            _authorRepository.CreateAuthor(authorDto);
            var cheepDto = new CheepDto
            {
                Message = "Bye, world!",
                TimeStamp = DateTime.UtcNow.ToString(),
                AuthorName = "Hans Hansen"
            };

            // Act
            _cheepRepository.CreateCheep(cheepDto);

            // Assert
            var cheep = _context.Cheeps.Include(cheep => cheep.Author).Single();
            Assert.Equal("Bye, world!", cheep.Text);
            Assert.Equal("Hans Hansen", cheep.Author.Name);
        }

        [Fact]
        public void GetCheeps_ReturnsAllCheeps()
        {
            // Arrange
            var author = new AuthorDto { Name = "Omar Semou", Email = "OmarSemou@hotmail.com" };
            _authorRepository.CreateAuthor(author);
            var cheep = new CheepDto { AuthorName = "Omar Semou", Message = "Testing 1 2 3", TimeStamp = DateTime.UtcNow.ToString() };
            _cheepRepository.CreateCheep(cheep);

            // Act
            var result = _cheepRepository.GetCheeps();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public void GetCheepsFromPage_ReturnsCorrectPage()
        {
            // Arrange
            var author = new AuthorDto { Name = "Darryl Davidson", Email = "merica4lyfe@usa.com" };
            _authorRepository.CreateAuthor(author);
            for (int i = 0; i < 50; i++)
            {
                var cheep = new CheepDto { AuthorName = "Darryl Davidson", Message = $"I ain't afriad {i}", TimeStamp = DateTime.UtcNow.ToString() };
                _cheepRepository.CreateCheep(cheep);
            }

            // Act
            var result = _cheepRepository.GetCheepsFromPage(2);

            // Assert
            Assert.Equal(18, result.Count()); 
        }

        [Fact]
        public void GetAuthorByName_ReturnsCorrectAuthor()
        {
            // Arrange
            var author = new AuthorDto { Name = "Spongebob Squarepants", Email = "mrgoofy@pants.com" };
            _authorRepository.CreateAuthor(author);

            // Act
            var result = _authorRepository.GetAuthorByName("Spongebob Squarepants");

            // Assert
            Assert.Equal("Spongebob Squarepants", result.Name);
        }

        [Fact]
        public void GetAuthorByEmail_ReturnsCorrectAuthor()
        {
            // Arrange
            var author = new AuthorDto { Name = "Karsten Pedersen", Email = "kp67@email.com" };
            _authorRepository.CreateAuthor(author);

            // Act
            var result = _authorRepository.GetAuthorByEmail("kp67@email.com");

            // Assert
            Assert.Equal("Karsten Pedersen", result.Name);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
