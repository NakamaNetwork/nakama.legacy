using TreasureGuide.Web.Models.UnitModels;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamUnitStubModel
    {
        public int UnitId { get; set; }
        public byte Position { get; set; }
    }

    public class TeamUnitDetailModel
    {
        public int UnitId { get; set; }
        public int Level { get; set; }
        public byte Position { get; set; }
        public byte? SpecialLevel { get; set; }
        public bool Sub { get; set; }
    }

    public class TeamUnitEditorModel
    {
        public int UnitId { get; set; }
        public byte Position { get; set; }
        public byte? SpecialLevel { get; set; }
        public bool Sub { get; set; }
    }
}
