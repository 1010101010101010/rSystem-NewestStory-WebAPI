using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class StoryRepository : IStoryRepository
    {
        private readonly rSystemContext _context;
        public StoryRepository(rSystemContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Story>> GetStories()
        {
            return _context.Stories.ToList();
        }

        public Task<Story> GetStory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Story> AddStory(Story story)
        {
            throw new NotImplementedException();
        }

        public Task<Story> DeleteStory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Story> UpdateStory(Story story)
        {
            throw new NotImplementedException();
        }
    }
}
