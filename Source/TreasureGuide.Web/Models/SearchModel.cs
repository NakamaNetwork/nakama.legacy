using System.Collections.Generic;

namespace TreasureGuide.Web.Models
{
    public class SearchModel
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string SortBy { get; set; }
        public bool SortDesc { get; set; }

        public const string SortId = "Id";
        public const string SortName = "Name";
        public const string SortType = "Type";
        public const string SortStage = "Stage";
        public const string SortClass = "Class";
        public const string SortCount = "Count";
        public const string SortScore = "Score";
        public const string SortDate = "Date";
        public const string SortUser = "User";
    }

    public class SearchResult<TType>
    {
        public int TotalResults { get; set; }
        public IEnumerable<TType> Results { get; set; }
    }
}
