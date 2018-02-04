using System.Collections.Generic;

namespace TreasureGuide.Sniffer.TeamImporters.Models
{
    public class RedditThread
    {
        public int StageId { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Threads { get; set; }
    }
}
