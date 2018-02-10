using System.Collections.Generic;

namespace TreasureGuide.Web.Models.BoxModels
{
    public class BoxUpdateModel
    {
        public int Id { get; set; }
        public IEnumerable<int> Added { get; set; }
        public IEnumerable<int> Removed { get; set; }
    }
}
