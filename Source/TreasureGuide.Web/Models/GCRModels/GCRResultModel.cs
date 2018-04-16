using System.Collections.Generic;

namespace TreasureGuide.Web.Models.GCRModels
{
    public class GCRResultModel
    {
        public IEnumerable<GCRDataModel> UnitIds { get; set; }
        public IEnumerable<GCRDataModel> StageIds { get; set; }
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

    public class GCRDataModel
    {
        public int? Id { get; set; }
        public int? Thumbnail { get; set; }
        public string Name { get; set; }
    }
}
