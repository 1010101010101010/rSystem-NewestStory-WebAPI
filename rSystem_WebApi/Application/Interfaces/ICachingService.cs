using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICachingService
    {
        void SetCache(string key, object value);
        T GetCache<T>(string key);
    }
}
