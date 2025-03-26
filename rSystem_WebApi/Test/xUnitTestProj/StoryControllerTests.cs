using Moq;
using System;
using System.Threading.Tasks;
using Api.Controllers;
using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace xUnitTestProj
{
    public class StoryControllerTests
    {
        private readonly Mock<IStoryServices> _mockStoryServices;
        private readonly StoryController _controller;

        public StoryControllerTests()
        {
            _mockStoryServices = new Mock<IStoryServices>();
            _controller = new StoryController(_mockStoryServices.Object);
        }

        [Fact]
        public async Task GetStories_ReturnsOkResult_WhenStoriesAreFound()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var storiesResponse = new PageResponse
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = 5,
                Stories = new List<StoryDto>
                {
                    new StoryDto { Title = "Story 1", Url = "http://story1.com" },
                    new StoryDto { Title = "Story 2", Url = "http://story2.com" }
                }
            };

            _mockStoryServices.Setup(service => service.GetStories(page, pageSize)).ReturnsAsync(storiesResponse);

            // Act
            var result = await _controller.GetStories(page, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PageResponse>(okResult.Value);
            Assert.Equal(2, returnValue.Stories.Count); // Verify the number of stories returned
        }

        [Fact]
        public async Task GetStories_ReturnsNoContent_WhenNoStoriesFound()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            _mockStoryServices.Setup(service => service.GetStories(page, pageSize)).ReturnsAsync((PageResponse)null);

            // Act
            var result = await _controller.GetStories(page, pageSize);

            // Assert
            var noContentResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode); // Verify 204 No Content
        }

        [Fact]
        public async Task GetStories_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            _mockStoryServices.Setup(service => service.GetStories(page, pageSize)).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _controller.GetStories(page, pageSize);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, errorResult.StatusCode); // Verify 500 Internal Server Error
        }
    }
}

