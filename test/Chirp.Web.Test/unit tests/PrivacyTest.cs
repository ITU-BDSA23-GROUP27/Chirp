using Chirp.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Razor.Test.unit_tests;

public class PrivacyTest
{
    private readonly PrivacyModel _privacyModel;

        private readonly Mock<ILogger<PrivacyModel>> _loggerMock = new Mock<ILogger<PrivacyModel>>();

        public PrivacyTest()
        {
            _privacyModel = new PrivacyModel(_loggerMock.Object);
        }

        [Fact]
        public async Task OnPostAuthenticateLogin_ShouldReturnActionResult()
        {
            // Arrange
            _loggerMock.Setup(logger => logger.LogInformation(It.IsAny<string>()));
            
            // Act
            var result = await _privacyModel.OnPostAuthenticateLogin();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            // Here we can add more assertions if we deem it necessary
        }

        [Fact]
        public async Task OnPostLogOut_ShouldReturnActionResult()
        {
            // Arrange

            // Act
            var result = await _privacyModel.OnPostLogOut();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            // Here we can add more assertions if we deem it necessary
        }

        [Fact]
        public void Constructor_ShouldInitializeLogger()
        {
            // Arrange
            var privacyModel = new PrivacyModel(_loggerMock.Object);

            // Act - No action needed for a constructor test

            // Assert
            Assert.NotNull(privacyModel);
            Assert.NotNull(privacyModel.Logger);
        }

        [Fact]
        public async Task OnPostAuthenticateLogin_ShouldHandleException()
        {
            // Arrange
            _loggerMock.Setup(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>()));

            // Act
            var result = await _privacyModel.OnPostAuthenticateLogin();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            // Here we can add more assertions if we deem it necessary
        }

        [Fact]
        public async Task OnPostLogOut_ShouldHandleException()
        {
            // Arrange
            _loggerMock.Setup(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>()));

            // Act
            var result = await _privacyModel.OnPostLogOut();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            // Here we can add more assertions if we deem it necessary
        }

    }
