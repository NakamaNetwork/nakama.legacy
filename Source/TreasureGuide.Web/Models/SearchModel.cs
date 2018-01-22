using System.Collections.Generic;

namespace TreasureGuide.Web.Models
{
    public class SearchModel
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string SortBy { get; set; }
        public bool SortDesc { get; set; }
    }

    public class SearchResult<TType>
    {
        public int TotalResults { get; set; }
        public IEnumerable<TType> Results { get; set; }
    }
}
