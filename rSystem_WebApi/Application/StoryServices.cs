using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Model;
using System;

namespace Services
{
    public class StoryServices : IStoryServices
    {
        private readonly ICachingService _cachingService;
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        string baseUrl = "https://hacker-news.firebaseio.com/v0";
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cachingService">ICachingService</param>
        /// <param name="http">HttpClient</param>
        /// <param name="configuration">IConfiguration</param>
        public StoryServices(ICachingService cachingService, HttpClient http, IConfiguration configuration)
        {
            _cachingService = cachingService;
            _http = http;
            _configuration = configuration;
        }
        /// <summary>
        /// Get Stories from External API or Cache then apply filter
        /// </summary>
        /// <param name="page">Page Number</param>
        /// <param name="pageSize">Items per page</param>
        /// <param name="searchTerm">searchTerm for storyId</param>
        /// <returns>A list of stories or null if not found.</returns>
        /// <exception cref="Exception">Thrown when fetching stories fails.</exception>
        public async Task<PageResponse> GetStories(int page, int pageSize, int searchTerm=0)
        {            
            baseUrl = _configuration["hacker-news-base-url"] ?? baseUrl;
            try
            {
                var storyIds = await GetTopStroies();
                if (searchTerm != 0) {
                    storyIds = storyIds.Where(id => id.ToString().StartsWith(searchTerm.ToString())).ToList();
                }
                var filteredIds =  storyIds.Skip(pageSize * (page - 1)).Take(pageSize).ToList();

                var fetchTasks = filteredIds.Select(id => GetStoryById(id)).ToArray();
                var stories = (await Task.WhenAll(fetchTasks)).Where(story => story != null).ToList();
           
                return new PageResponse
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)storyIds.Count() / pageSize),
                    Stories = stories
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get stories from external API :{ex.Message}");
            }
        }

        /// <summary>
        /// Get story data by Id.
        /// </summary>
        /// <param name="storyId">story Id.</param>
        /// <returns>Story data</returns>
        private async Task<Story?> GetStoryById(int storyId)
        {
            try
            {
                var storyRes = await _http.GetAsync($"{baseUrl}/item/{storyId}.json?print=pretty");

                if (storyRes.IsSuccessStatusCode)
                {
                    var res = await storyRes.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Story>(res);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching story {storyId}: {ex.Message}");
            }

            return null;
        }
        /// <summary>
        /// Get All Top Stories Ids
        /// </summary>
        /// <returns>List of Story Ids</returns>
        private async Task<List<int>> GetTopStroies()
        {
            var cachedStories = _cachingService.GetCache<IEnumerable<int>>("topstories_CacheKey");
            if (cachedStories == null)
            {
                var topstoriesurl = $"{baseUrl}/topstories.json?print=pretty";
                var response = await _http.GetAsync(topstoriesurl);
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    cachedStories = JsonConvert.DeserializeObject<IEnumerable<int>>(res);
                    if (cachedStories != null)
                    {
                        _cachingService.SetCache("topstories_CacheKey", cachedStories);
                    }
                }
            }
            return cachedStories.ToList();
        }

    }
}
