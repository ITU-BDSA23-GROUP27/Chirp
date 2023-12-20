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

public class UserTimelineTest
{
    private readonly UserTimelineModel _userTimelineModel;
    
    private readonly Mock<ICheepRepository> _cheepRepositoryMock = new Mock<ICheepRepository>();
    private readonly Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();
    private readonly Mock<IFollowerRepository> _followerRepositoryMock = new Mock<IFollowerRepository>();
    private readonly Mock<IReactionRepository> _reactionRepositoryMock = new Mock<IReactionRepository>();
    private readonly Mock<IValidator<CheepDto>> _validatorMock = new Mock<IValidator<CheepDto>>();

    public UserTimelineTest()
    {
        _userTimelineModel = new UserTimelineModel(
            _cheepRepositoryMock.Object,
            _userRepositoryMock.Object,
            _followerRepositoryMock.Object,
            _reactionRepositoryMock.Object,
            _validatorMock.Object
        );
        
        _userTimelineModel.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };
    }
    
    
    [Fact]
    public async Task OnGet_UserIsAuthenticated_ReturnsPageResult()
    {
        // Arrange

        var userClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Authentication, "true")
        };
        
        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "mock"));
        
        _userTimelineModel.PageContext = new PageContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = userPrincipal
            }
        };

        // Act
        var result = await _userTimelineModel.OnGet("testUser");

        // Assert
        Assert.IsType<PageResult>(result);
    }

    [Fact]
    public async Task OnGet_UserIsNotAuthenticated_ReturnsRedirectToPageResult()
    {
        // Arrange
        
        // Act
        var result = await _userTimelineModel.OnGet("testUser");

        // Assert
        var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Public", redirectToPageResult.PageName);
    }
    
    [Fact]
    public async Task GetTotalPages_ReturnsCorrectTotalPages()
    {
        // Arrange
        // We could change the amount of dto objects returned from the repository to test the logic
        _cheepRepositoryMock.Setup(repo => repo.GetCheepsFromUser(It.IsAny<string>()))
            .ReturnsAsync(new List<CheepDto>());
        
        // Act
        var totalPages = await _userTimelineModel.GetTotalPages("testUser");

        // Assert
        Assert.Equal(0, totalPages);
    }

    [Fact]
    public async Task OnPostFollow_WithValidData_ReturnsRedirectToPageResult()
    {
        // Arrange

        // Act
        var result = await _userTimelineModel.OnPostFollow("userName", "followerName");

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
    }

    [Fact]
    public async Task OnPostLikeCheep_WithValidData_ReturnsRedirectToPageResult()
    {
        // Arrange

        // Act
        var result = await _userTimelineModel.OnPostLikeCheep(Guid.NewGuid(), "userName");

        // Assert
        Assert.IsType<RedirectToPageResult>(result);
    }

    [Fact]
    public async Task GetLikeCount_ReturnsCorrectLikeCount()
    {
        // Arrange

        // Act
        var likeCount = await _userTimelineModel.GetLikeCount(Guid.NewGuid());

        // Assert
        Assert.Equal(0, likeCount);
    }

    [Fact]
    public async Task HasUserLikedCheep_ReturnsCorrectLikeStatus()
    {
        // Arrange

        // Act
        var hasLiked = await _userTimelineModel.HasUserLikedCheep(Guid.NewGuid(), "userName");

        // Assert
        Assert.False(hasLiked);
    }
}