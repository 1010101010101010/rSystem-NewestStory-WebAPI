using Services.Model;

namespace Services.Interfaces
{
    public interface IStoryServices
    {
        Task<PageResponse> GetStories(int page, int pageSize, int searchTerm);
    }
}
