using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IStoryRepository
    {
        Task<IEnumerable<Story>> GetStories();
        Task<Story> GetStory(int id);
        Task<Story> AddStory(Story story);
        Task<Story> UpdateStory(Story story);
        Task<Story> DeleteStory(int id);
    }
}
