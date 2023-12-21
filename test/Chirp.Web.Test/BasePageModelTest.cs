using Chirp.Core;
using Chirp.Core.DTOs;
using Chirp.Web.Pages;
using FluentValidation;
using Moq;

namespace Razor.Test.unit_tests;

public class BasePageModelTest
{
    private readonly BasePageModel _basePageModel;

    private readonly Mock<IValidator<CheepDto>> _validatorMock = new Mock<IValidator<CheepDto>>();
    private readonly Mock<ICheepRepository> _cheepRepositoryMock = new Mock<ICheepRepository>();
    private readonly Mock<IFollowerRepository> _followerRepositoryMock = new Mock<IFollowerRepository>();
    private readonly Mock<IReactionRepository> _reactionRepositoryMock = new Mock<IReactionRepository>();
    private readonly Mock<IValidator<ReactionDto>> _reactionValidatorMock = new Mock<IValidator<ReactionDto>>();

    public BasePageModelTest()
    {
        _basePageModel = new BasePageModel();
    }

    [Fact]
    public async Task HandleFollow_WithNullAuthorName_ShouldThrowArgumentNullException()
    {
        // Arrange

        // Act and Assert - using lambda so same method can be used for both tests
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _basePageModel.HandleFollow(null!, "followerName", _followerRepositoryMock.Object));
    }

    [Fact]
    public async Task HandleFollow_WithNullFollowerName_ShouldThrowArgumentNullException()
    {
        // Arrange

        // Act and Assert - using lambda so same method can be used for both tests
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _basePageModel.HandleFollow("authorName", null!, _followerRepositoryMock.Object));
    }

    [Fact]
    public async Task HandleLike_WithNullUserName_ShouldThrowArgumentNullException()
    {
        // Arrange

        // Act and Assert - using lambda so same method can be used for both tests
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _basePageModel.HandleLike(Guid.NewGuid(), null!, _reactionRepositoryMock.Object));
    }
}
