using Chirp.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Razor.Test.unit_tests;

public class ErrorTest
{
    private readonly ErrorModel _errorModel;

        private readonly Mock<ILogger<ErrorModel>> _loggerMock = new Mock<ILogger<ErrorModel>>();

        public ErrorTest()
        {
            _errorModel = new ErrorModel(_loggerMock.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeLogger()
        {
            // Arrange
            var errorModel = new ErrorModel(_loggerMock.Object);

            // Act

            // Assert
            Assert.NotNull(errorModel);
            Assert.NotNull(errorModel.Logger);
        }

        [Fact]
        public void OnGet_ShouldSetRequestId()
        {
            // Arrange

            // Act
            _errorModel.OnGet();

            // Assert
            Assert.NotNull(_errorModel.RequestId);
        }

        [Fact]
        public async void OnPostAuthenticateLogin_ShouldReturnActionResult()
        {
            // Arrange

            // Act
            var result = await _errorModel.OnPostAuthenticateLogin();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async void OnPostLogOut_ShouldReturnActionResult()
        {
            // Arrange

            // Act
            var result = await _errorModel.OnPostLogOut();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }

    }
