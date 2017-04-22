using TreasureGuide.Web.Models.UnitModels;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamUnitStubModel
    {
        public UnitStubModel Unit { get; set; }
    }

    public class TeamUnitDetailModel
    {
        public UnitDetailModel Unit { get; set; }
        public int Position { get; set; }
        public int SpecialLevel { get; set; }
        public bool Sub { get; set; }
    }

    public class TeamUnitEditorModel
    {
        public int UnitId { get; set; }
        public int Position { get; set; }
        public int SpecialLevel { get; set; }
        public bool Sub { get; set; }
    }
}
