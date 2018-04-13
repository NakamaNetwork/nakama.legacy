using System.Collections.Generic;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.GCRModels
{
    public class GCRResultModel
    {
        public IEnumerable<int> UnitIds { get; set; }
        public IEnumerable<int> StageIds { get; set; }
        public IEnumerable<GCRTableModel> Teams { get; set; }
    }

    public class GCRTableModel
    {
        public int Id { get; set; }
        public int? LeaderId { get; set; }
        public int StageId { get; set; }
        public bool F2P { get; set; }
        public bool Global { get; set; }
        public bool Video { get; set; }
    }
}
