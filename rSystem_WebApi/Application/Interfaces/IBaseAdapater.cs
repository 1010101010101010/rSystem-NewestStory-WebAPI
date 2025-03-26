using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBaseAdapater
    {
        Task<List<StoryDto>?> GetStories(int page, int pageSize);
    }
}
