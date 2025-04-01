using Moq;
using Api.Controllers;
using Services.Interfaces;
using Services.Model;
using Microsoft.AspNetCore.Mvc;


namespace xUnitTestProj
{
    public class StoryControllerTests
    {
        private readonly Mock<IStoryServices> _mockStoryServices;
        private readonly StoryController _storyController;

        public StoryControllerTests()
        {
            _mockStoryServices = new Mock<IStoryServices>();
            _storyController = new StoryController(_mockStoryServices.Object);
        }

        [Fact]
        public async Task GetStories_ReturnsOkResult_WithStories()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var stories = new PageResponse
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = 5,
                Stories = new List<Story>
                {
                    new Story { id = 43543241, title = "Show HN: Nue – Apps lighter than a React button" },
                    new Story { id = 43511529, title = "The Guardian flourishes without a paywall" }
                }
            };

            _mockStoryServices.Setup(service => service.GetStories(page, pageSize))
                              .ReturnsAsync(stories);

            // Act
            var result = await _storyController.GetStories(page, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PageResponse>(okResult.Value);
            Assert.Equal(page, returnValue.Page);
            Assert.Equal(pageSize, returnValue.PageSize);
            Assert.Equal(2, returnValue.Stories.Count);
        }

        [Fact]
        public async Task GetStories_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            _mockStoryServices.Setup(service => service.GetStories(page, pageSize))
                              .ThrowsAsync(new Exception("Internal server error"));

            // Act
            var result = await _storyController.GetStories(page, pageSize);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal server error", objectResult.Value);
        }
    }
}
