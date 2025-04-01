using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace xUnitTestProj
{
    public class ErrorControllerTests
    {
        private readonly Mock<ILogger<ErrorController>> _mockLogger;
        private readonly ErrorController _controller;

        public ErrorControllerTests()
        {
            _mockLogger = new Mock<ILogger<ErrorController>>();
            _controller = new ErrorController(_mockLogger.Object);
        }

        [Fact]
        public void LogError_ShouldReturnBadRequest_WhenErrorMessageIsNullOrEmpty()
        {
            // Act
            var result = _controller.LogError(null); 
            var result2 = _controller.LogError(""); 

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<BadRequestObjectResult>(result2);
        }
       
    }
}
