using System.Collections.Generic;

namespace TreasureGuide.Web.Models.Teams
{
    public class TeamBrowserModel
    {
        public int Id { get; set; }
        public int TeamScore { get; set; }
        public string Description { get; set; }
        public bool Global { get; set; }
        public IEnumerable<int> Units { get; set; }
    }
}