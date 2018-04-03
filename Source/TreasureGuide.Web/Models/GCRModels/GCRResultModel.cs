using System.Collections.Generic;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.GCRModels
{
    public class GCRResultModel
    {
        public IEnumerable<int> UnitIds { get; set; }
        public IEnumerable<int> StageIds { get; set; }
        public IEnumerable<GCRTable> Teams { get; set; }
    }
}
