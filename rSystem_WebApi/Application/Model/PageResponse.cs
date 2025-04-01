using Services.Model;

namespace Services.Model
{
    public class PageResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public List<Story> Stories { get; set; }
    }
}
