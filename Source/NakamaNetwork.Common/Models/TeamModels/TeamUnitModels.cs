using NakamaNetwork.Entities;
using NakamaNetwork.Entities.EnumTypes;
using NakamaNetwork.Entities.Interfaces;

namespace TreasureGuide.Common.Models.TeamModels
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
