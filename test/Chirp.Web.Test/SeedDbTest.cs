using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Razor.Test.unit_tests;

public class SeedDbTest
{
    private readonly ChirpContext _context;

    public SeedDbTest()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ChirpContext>()
            .UseSqlite(connection)
            .Options;

        _context = new ChirpContext(options);
        _context.Database.Migrate();
    }

    [Fact]
    public async Task OnPostSeedDatabase_ShouldSeedDatabase()
    {
        // Arrange
        var seedDbModel = new SeedDbModel(_context);

        // Act
        var result = await seedDbModel.OnPostSeedDatabase();

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Public", ((RedirectToPageResult)result).PageName);
        // Room to add more assertions if necessary
    }

    [Fact]
    public async Task OnPostSeedDatabase2_ShouldSeedDatabase2()
    {
        // Arrange
        var seedDbModel = new SeedDbModel(_context);

        // Act
        var result = await seedDbModel.OnPostSeedDatabase2();

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Public", ((RedirectToPageResult)result).PageName);
        // Room to add more assertions if necessary
    }

    [Fact]
    public void OnPostClearDatabase_ShouldClearDatabase()
    {
        // Arrange
        var seedDbModel = new SeedDbModel(_context);

        // Act
        var result = seedDbModel.OnPostClearDatabase();

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Public", ((RedirectToPageResult)result).PageName);
        // Room to add more assertions if necessary
    }


}