using Chirp.Infrastructure;
using Chirp.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Razor.Test.unit_tests;

public class SeedDBTest
{
    private readonly DbContextOptions<ChirpContext> _options;
    private readonly ChirpContext _context;

    public SeedDBTest()
    {
        _options = new DbContextOptionsBuilder<ChirpContext>()
            .UseModel(new Model()) // TODO Should use a database here - possibly three different to get seperate data 
            .Options;

        _context = new ChirpContext(_options);
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