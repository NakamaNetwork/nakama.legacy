using System.Collections.Generic;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.GCRModels
{
    public class GCREditorModel
    {
        public IEnumerable<GCRUnitEditModel> Units { get; set; }
        public IEnumerable<GCRStageEditModel> Stages { get; set; }
    }

    public class GCRResultModel
    {
        public IEnumerable<GCRDataModel> Units { get; set; }
        public IEnumerable<GCRDataModel> Stages { get; set; }
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
        public UnitType? Color { get; set; }
    }

    public abstract class GCRAbstractEditModel
    {
        public int Order { get; set; }
        public string Name { get; set; }
    }

    public class GCRUnitEditModel : GCRAbstractEditModel
    {
        public int UnitId { get; set; }
    }

    public class GCRStageEditModel : GCRAbstractEditModel
    {
        public int StageId { get; set; }
    }
}
