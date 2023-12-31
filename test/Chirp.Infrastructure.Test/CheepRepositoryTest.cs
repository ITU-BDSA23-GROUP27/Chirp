using System.Globalization;
using Chirp.Core.DTOs;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure.Test;

public class CheepRepositoryTest
{
    private readonly ChirpContext _context;
    private readonly CheepRepository _cheepRepository;
    private readonly UserRepository _userRepository;

    public CheepRepositoryTest()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ChirpContext>()
            .UseSqlite(connection)
            .Options;

        _context = new ChirpContext(options);
        _context.Database.Migrate();

        _cheepRepository = new CheepRepository(_context);
        _userRepository = new UserRepository(_context);
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
            TimeStamp = DateTime.UtcNow.ToString(CultureInfo.CurrentCulture),
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
}