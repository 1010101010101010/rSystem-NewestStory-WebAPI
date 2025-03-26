using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
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
           return (T)_memoryCache.Get(key);
        }

        public void SetCache(string key, object value)
        {
            _memoryCache.Set(key, value);
        }
    }
}
