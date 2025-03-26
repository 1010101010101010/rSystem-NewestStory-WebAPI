using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace xUnitTestProj
{
    public class StoryServicesTests
    {
        private readonly Mock<ICachingService> _mockCachingService;
        private readonly Mock<IStoryRepository> _mockStoryRepository;
        private readonly Mock<IBaseAdapater> _mockAdapter;
        private readonly StoryServices _storyServices;

        public StoryServicesTests()
        {
            _mockCachingService = new Mock<ICachingService>();
            _mockStoryRepository = new Mock<IStoryRepository>();
            _mockAdapter = new Mock<IBaseAdapater>();
            _storyServices = new StoryServices(_mockCachingService.Object, _mockStoryRepository.Object, _mockAdapter.Object);
        }

        [Fact]
        public async Task GetStories_ShouldReturnCachedStories_WhenCacheIsAvailable()
        {
            // Arrange
            IEnumerable<Story> cachedStories = new List<Story>
            {
                new Story { Title = "Story 1", StoryUrl = "http://story1.com" },
                new Story { Title = "Story 2", StoryUrl = "http://story2.com" }
            };

            _mockCachingService.Setup(service => service.GetCache<IEnumerable<Story>>("GetStories")).Returns(cachedStories);

            // Act
            var result = await _storyServices.GetStories(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Stories.Count);
            Assert.Equal("Story 1", result.Stories[0].Title);
            Assert.Equal("Story 2", result.Stories[1].Title);
        }

        [Fact]
        public async Task GetStories_ShouldReturnStoriesFromRepository_WhenCacheIsEmpty()
        {
            // Arrange
            _mockCachingService.Setup(service => service.GetCache<IEnumerable<Story>>("GetStories")).Returns(null as IEnumerable<Story>);

            var storiesFromRepository = new List<Story>
            {
                new Story { Title = "Repo Story 1", StoryUrl = "http://repoStory1.com" },
                new Story { Title = "Repo Story 2", StoryUrl = "http://repoStory2.com" }
            };
            _mockStoryRepository.Setup(repo => repo.GetStories()).ReturnsAsync(storiesFromRepository);

            // Act
            var result = await _storyServices.GetStories(1, 10);

            // Assert
            _mockCachingService.Verify(service => service.SetCache("GetStories", It.IsAny<IEnumerable<Story>>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(2, result.Stories.Count);
            Assert.Equal("Repo Story 1", result.Stories[0].Title);
            Assert.Equal("Repo Story 2", result.Stories[1].Title);
        }

        [Fact]
        public async Task GetStories_ShouldMergeExternalApiData_WhenExternalApiHasData()
        {
            // Arrange
            var cachedStories = new List<Story>
            {
                new Story { Title = "Story 1", StoryUrl = "http://story1.com" }
            };
            _mockCachingService.Setup(service => service.GetCache<IEnumerable<Story>>("GetStories")).Returns(cachedStories);

            var externalApiData = new List<StoryDto>
            {
                new StoryDto { Title = "External Story 1", Url = "http://external1.com" }
            };
            _mockAdapter.Setup(adapter => adapter.GetStories(1, 10)).ReturnsAsync(externalApiData);

            // Act
            var result = await _storyServices.GetStories(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Stories.Count); // One from cache, one from external API
            Assert.Equal("Story 1", result.Stories[0].Title);
            Assert.Equal("External Story 1", result.Stories[1].Title);
        }

        [Fact]
        public async Task GetStories_ShouldReturnEmptyList_WhenNoStoriesFound()
        {
            // Arrange
            _mockCachingService.Setup(service => service.GetCache<IEnumerable<Story>>("GetStories")).Returns(null as IEnumerable<Story>);
            _mockStoryRepository.Setup(repo => repo.GetStories()).ReturnsAsync(new List<Story>());

            // Act
            var result = await _storyServices.GetStories(1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Stories);
        }
    }
}
