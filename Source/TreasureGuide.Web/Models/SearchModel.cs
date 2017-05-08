using System.Collections.Generic;

namespace TreasureGuide.Web.Models
{
    public class SearchModel
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 25;
    }

    public class SearchResult<TType>
    {
        public int TotalPages { get; set; }
        public IEnumerable<TType> Results { get; set; }
    }
}
