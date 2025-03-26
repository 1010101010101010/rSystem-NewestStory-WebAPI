using Application.Adapters;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StoryServices : IStoryServices
    {
        private readonly ICachingService _cachingService;
        private readonly IStoryRepository _storyRepository;
        private readonly IBaseAdapater _adapater;
        public StoryServices(ICachingService cachingService, IStoryRepository storyRepository, IBaseAdapater adapater)
        {
            _cachingService = cachingService;
            _storyRepository = storyRepository;
            _adapater = adapater;
        }
        public async Task<PageResponse> GetStories(int page, int pageSize)
        {
            var cachedStoriesValues = _cachingService.GetCache<IEnumerable<Story>>("GetStories");
            IEnumerable<Story>? res = new List<Story>();
            if (cachedStoriesValues != null)
            {
                res = cachedStoriesValues as IEnumerable<Story>;
            }
            else
            {
                res = await _storyRepository.GetStories();
                _cachingService.SetCache("GetStories", res);
            }
            var stories = res?.Skip(pageSize * (page - 1)).Take(pageSize).Select(x => new StoryDto
            {
                Title = x.Title,
                Url = x.StoryUrl
            }).ToList();

            var externalApiDatas = await _adapater.GetStories(page, pageSize);
            if (externalApiDatas != null && externalApiDatas.Any())
            {
                stories?.AddRange(externalApiDatas);
            }

            return new PageResponse
            {
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)res.Count() / pageSize),
                Stories = stories
            };
        }

    }
}
