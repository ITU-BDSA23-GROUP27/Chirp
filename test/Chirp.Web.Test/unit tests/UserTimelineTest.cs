using System.Security.Claims;
using System.Security.Principal;
using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Web.Pages;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using Xunit;
namespace Razor.Test.unit_tests;

public class UserTimelineTest
{
    private readonly Mock<ICheepRepository> _cheepRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IFollowerRepository> _followerRepositoryMock;
    private readonly Mock<IReactionRepository> _reactionRepositoryMock;
    private readonly Mock<IValidator<CheepDto>> _validatorMock;

    public UserTimelineTest()
    {
        _cheepRepositoryMock = new Mock<ICheepRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _followerRepositoryMock = new Mock<IFollowerRepository>();
        _reactionRepositoryMock = new Mock<IReactionRepository>();
        _validatorMock = new Mock<IValidator<CheepDto>>();
    }
    
    
    [Fact]
        public async Task OnGet_UserIsAuthenticated_ReturnsPageResult()
        {
            // Arrange
            var cheepRepositoryMock = new Mock<ICheepRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var followerRepositoryMock = new Mock<IFollowerRepository>();
            var reactionRepositoryMock = new Mock<IReactionRepository>();
            var validatorMock = new Mock<IValidator<CheepDto>>();

            var userTimelineModel = new UserTimelineModel(
                cheepRepositoryMock.Object,
                userRepositoryMock.Object,
                followerRepositoryMock.Object,
                reactionRepositoryMock.Object,
                validatorMock.Object
            );

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Authentication, "true")
            };
            
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims, "mock"));
            
            userTimelineModel.PageContext = new PageContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = userPrincipal
                }
            };

            // Act
            var result = await userTimelineModel.OnGet("testUser");

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnGet_UserIsNotAuthenticated_ReturnsRedirectToPageResult()
        {
            // Arrange
            var cheepRepositoryMock = new Mock<ICheepRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var followerRepositoryMock = new Mock<IFollowerRepository>();
            var reactionRepositoryMock = new Mock<IReactionRepository>();
            var validatorMock = new Mock<IValidator<CheepDto>>();

            var userTimelineModel = new UserTimelineModel(
                cheepRepositoryMock.Object,
                userRepositoryMock.Object,
                followerRepositoryMock.Object,
                reactionRepositoryMock.Object,
                validatorMock.Object
            );

            userTimelineModel.PageContext = new PageContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity()),
                }
            };
            
            // Act
            var result = await userTimelineModel.OnGet("testUser");

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Public", redirectToPageResult.PageName);
        }
        
        [Fact]
        public async Task GetTotalPages_ReturnsCorrectTotalPages()
        {
            // Arrange
            var cheepRepositoryMock = new Mock<ICheepRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var followerRepositoryMock = new Mock<IFollowerRepository>();
            var reactionRepositoryMock = new Mock<IReactionRepository>();
            var validatorMock = new Mock<IValidator<CheepDto>>();

            // We could change the amount of dto objects returned from the repository to test the logic
            cheepRepositoryMock.Setup(repo => repo.GetCheepsFromUser(It.IsAny<string>()))
                .ReturnsAsync(new List<CheepDto> {});

            var userTimelineModel = new UserTimelineModel(
                cheepRepositoryMock.Object,
                userRepositoryMock.Object,
                followerRepositoryMock.Object,
                reactionRepositoryMock.Object,
                validatorMock.Object
            );

            // Act
            var totalPages = await userTimelineModel.GetTotalPages("testUser");

            // Assert
            Assert.Equal(0, totalPages);
        }
        
        [Fact]
        public async Task OnPostLogOut_ReturnsRedirectToPageResult()
        {
            // Arrange
            var cheepRepositoryMock = new Mock<ICheepRepository>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var followerRepositoryMock = new Mock<IFollowerRepository>();
            var reactionRepositoryMock = new Mock<IReactionRepository>();
            var validatorMock = new Mock<IValidator<CheepDto>>();

            var userTimelineModel = new UserTimelineModel(
                cheepRepositoryMock.Object,
                userRepositoryMock.Object,
                followerRepositoryMock.Object,
                reactionRepositoryMock.Object,
                validatorMock.Object
            );

            userTimelineModel.PageContext = new PageContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal()
                }
            };
            
            // Act
            var result = await userTimelineModel.OnPostLogOut();

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectToPageResult.PageName); // Adjust if your log out redirects to a different page
        }
    
        
        [Fact]
        public async Task OnPostCheep_WithValidData_ReturnsRedirectToPageResult()
        {
            // Arrange
            var userTimelineModel = CreateUserTimelineModel();

            // Act
            var result = await userTimelineModel.OnPostCheep();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            // Add additional assertions if needed
        }

        [Fact]
        public async Task OnPostFollow_WithValidData_ReturnsRedirectToPageResult()
        {
            // Arrange
            var userTimelineModel = CreateUserTimelineModel();

            // Act
            var result = await userTimelineModel.OnPostFollow("userName", "followerName");

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostLikeCheep_WithValidData_ReturnsRedirectToPageResult()
        {
            // Arrange
            var userTimelineModel = CreateUserTimelineModel();

            // Act
            var result = await userTimelineModel.OnPostLikeCheep(Guid.NewGuid(), "userName");

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task GetLikeCount_ReturnsCorrectLikeCount()
        {
            // Arrange
            var userTimelineModel = CreateUserTimelineModel();

            // Act
            var likeCount = await userTimelineModel.GetLikeCount(Guid.NewGuid());

            // Assert
            Assert.Equal(0, likeCount);
        }

        [Fact]
        public async Task HasUserLikedCheep_ReturnsCorrectLikeStatus()
        {
            // Arrange
            var userTimelineModel = CreateUserTimelineModel();

            // Act
            var hasLiked = await userTimelineModel.HasUserLikedCheep(Guid.NewGuid(), "userName");

            // Assert
            Assert.False(hasLiked);
        }

        [Fact]
        public async Task OnPostAuthenticateLogin_ReturnsCorrectActionResult()
        {
            // Arrange
            var userTimelineModel = CreateUserTimelineModel();

            // Act
            var result = await userTimelineModel.OnPostAuthenticateLogin();

            // Assert
            Assert.IsType<RedirectToPageResult>(result); 
        }

        [Fact]
        public async Task OnPostLogOut_ReturnsCorrectActionResult()
        {
            // Arrange
            var userTimelineModel = CreateUserTimelineModel();

            // Act
            var result = await userTimelineModel.OnPostLogOut();

            // Assert
            Assert.IsType<RedirectToPageResult>(result); 
        }
        
        
        
        private UserTimelineModel CreateUserTimelineModel()
        {
            return new UserTimelineModel(
                _cheepRepositoryMock.Object,
                _userRepositoryMock.Object,
                _followerRepositoryMock.Object,
                _reactionRepositoryMock.Object,
                _validatorMock.Object
            );
        }
}