using TreasureGuide.Entities;
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
        public IndividualUnitFlags? Flags { get; set; }
        public bool Sub { get; set; }
    }

    public class TeamUnitEditorModel : TeamUnitStubModel, ISubItem
    {
        public IndividualUnitFlags? Flags { get; set; }
        public bool Sub { get; set; }
    }
}
