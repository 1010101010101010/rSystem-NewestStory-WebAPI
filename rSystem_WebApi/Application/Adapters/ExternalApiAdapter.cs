using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Newtonsoft.Json;

namespace Application.Adapters
{
    public class ExternalApiAdapter:IBaseAdapater
    {
        private readonly HttpClient _http;
        string baseUrl = "https://google.com/demo/stories";
        public ExternalApiAdapter(HttpClient http)
        {
            _http = http;
        }
        public async Task<List<StoryDto>?> GetStories(int page, int pageSize)
        {
            try
            {                
                var response = await _http.GetAsync(baseUrl);
                if (response.IsSuccessStatusCode)
                {
                    var res = await response.Content.ReadAsStringAsync();
                    var storyData = JsonConvert.DeserializeObject<IEnumerable<StoryDto>>(res);
                    return storyData?.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception($"Failed to get stories from external API :{ex.Message}");
            }
            return null;
        }
    }
}
