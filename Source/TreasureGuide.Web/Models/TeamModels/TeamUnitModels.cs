using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamUnitStubModel
    {
        public int UnitId { get; set; }
        public byte Position { get; set; }
    }

    public class TeamUnitDetailModel : TeamUnitStubModel, ISubItem
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public byte? Special { get; set; }
        public bool Sub { get; set; }
    }

    public class TeamUnitEditorModel : TeamUnitStubModel, ISubItem
    {
        public byte? Special { get; set; }
        public bool Sub { get; set; }
    }
}
