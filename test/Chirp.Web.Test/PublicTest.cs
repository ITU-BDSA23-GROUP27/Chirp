using System.Security.Claims;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Web.Pages;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;

namespace Razor.Test.unit_tests;

public class PublicTest
{
    private readonly PublicModel _publicModel;

    private readonly Mock<ICheepRepository> _cheepRepositoryMock = new Mock<ICheepRepository>();
    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
    private readonly Mock<IFollowerRepository> _followerRepositoryMock = new Mock<IFollowerRepository>();
    private readonly Mock<IReactionRepository> _reactionRepositoryMock = new Mock<IReactionRepository>();
    private readonly Mock<IValidator<CheepDto>> _validatorMock = new Mock<IValidator<CheepDto>>();
    
    public PublicTest()
    {
        _publicModel = new PublicModel(
            _cheepRepositoryMock.Object,
            _userRepositoryMock.Object,
            _followerRepositoryMock.Object,
            _reactionRepositoryMock.Object,
            _validatorMock.Object
        );
        
        _publicModel.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };

        //_publicModel.Url = new UrlHelperFactory().GetUrlHelper(_publicModel.PageContext);
    }
    
    [Fact]
    public async Task OnGet_ShouldReturnPageResult()
    {
        // Arrange
        _cheepRepositoryMock.Setup(repo => repo.GetCheepsFromPage(It.IsAny<int>())).ReturnsAsync(new List<CheepDto>());

        // Act
        var result = await _publicModel.OnGet();

        // Assert
        Assert.IsType<PageResult>(result);
        // We can add more assertions here if necessary
    }
    
    [Fact]
    public async Task OnPostFollow_ShouldReturnActionResult()
    {
        // Arrange
        _followerRepositoryMock.Setup(repo => repo.AddOrRemoveFollower(It.IsAny<string>(), It.IsAny<string>()));

        // Act
        var result = await _publicModel.OnPostFollow("userName", "followerName");

        // Assert
        Assert.IsAssignableFrom<ActionResult>(result);
        // We can add more assertions here if necessary
    }

    [Fact]
    public async Task OnPostLikeCheep_ShouldReturnActionResult()
    {
        // Arrange
        _reactionRepositoryMock.Setup(repo => repo.LikeCheep(It.IsAny<Guid>(), It.IsAny<string>()));

        // Act
        var result = await _publicModel.OnPostLikeCheep(Guid.NewGuid(), "userName");

        // Assert
        Assert.IsAssignableFrom<ActionResult>(result);
        // We can add more assertions here if necessary
    }

    [Fact]
    public async Task GetLikeCount_ShouldReturnInt()
    {
        // Arrange
        _reactionRepositoryMock.Setup(repo => repo.GetLikeCount(It.IsAny<Guid>())).ReturnsAsync(5);

        // Act
        var result = await _publicModel.GetLikeCount(Guid.NewGuid());

        // Assert
        Assert.Equal(5, result);
        // We can add more assertions here if necessary
    }

    [Fact]
    public async Task HasUserLikedCheep_ShouldReturnBool()
    {
        // Arrange
        _reactionRepositoryMock.Setup(repo => repo.HasUserLiked(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);

        // Act
        var result = await _publicModel.HasUserLikedCheep(Guid.NewGuid(), "userName");

        // Assert
        Assert.True(result);
        // We can add more assertions here if necessary
    }
}