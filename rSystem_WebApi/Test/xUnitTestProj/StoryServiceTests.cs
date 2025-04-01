using Moq;
using Newtonsoft.Json;
using Services;
using Services.Interfaces;
using Services.Model;
using Microsoft.Extensions.Configuration;

namespace xUnitTestProj
{
    public class StoryServicesTests
    {
        private readonly Mock<ICachingService> _mockCachingService;
        private readonly Mock<HttpClient> _mockHttpClient;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly StoryServices _storyServices;

        public StoryServicesTests()
        {
            _mockCachingService = new Mock<ICachingService>();
            _mockHttpClient = new Mock<HttpClient>();
            _mockConfiguration = new Mock<IConfiguration>();

            _storyServices = new StoryServices(_mockCachingService.Object, _mockHttpClient.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task GetStories_Returns_Valid_Stories()
        {
            // Arrange
            var page = 1;
            var pageSize = 2;
            var mockStoryIds = new List<int> { 43544979, 43543241, 43543235, 43504940, 43518462, 43511529, 43512470 };
            var mockStoryData = new Story { id = 43544979, title = "Self-Hosting like it's 2025" };

            // Mocking IConfiguration to return the base URL
            _mockConfiguration.Setup(config => config["hacker-news-base-url"]).Returns("https://hacker-news.firebaseio.com/v0/");

            // Mock GetTopStroies to return story IDs
            _mockCachingService.Setup(service => service.GetCache<IEnumerable<int>>("topstories_CacheKey"))
                               .Returns(mockStoryIds);

            // Mock GetStoryById to return mock story data for the given ID
            //_mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>()))
            //               .ReturnsAsync(new HttpResponseMessage
            //               {
            //                   Content = new StringContent(JsonConvert.SerializeObject(mockStoryData))
            //               });

            // Act
            var result = await _storyServices.GetStories(page, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(page, result.Page);
            Assert.Equal(pageSize, result.PageSize);
            Assert.Equal("Self-Hosting like it's 2025", result.Stories[0].title);
        }        
    }
}
