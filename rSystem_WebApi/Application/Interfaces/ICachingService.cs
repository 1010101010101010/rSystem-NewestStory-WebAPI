
namespace Services.Interfaces
{
    public interface ICachingService
    {
        void SetCache(string key, object value);
        T GetCache<T>(string key);
    }
}
