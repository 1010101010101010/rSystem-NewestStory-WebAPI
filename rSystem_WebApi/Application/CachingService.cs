using Microsoft.Extensions.Caching.Memory;
using Services.Interfaces;

namespace Services
{
    public class CachingService : ICachingService
    {
        private readonly IMemoryCache _memoryCache;
        public CachingService(IMemoryCache memoryCache)
        { 
            _memoryCache = memoryCache;
        }
        public T GetCache<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T value))
            {
                return value;
            }
            return default(T);
        }

        public void SetCache(string key, object value)
        {
            _memoryCache.Set(key, value);
        }
    }
}
