using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamGenericSlotStubModel 
    {
        public UnitRole Role { get; set; }
        public UnitType Type { get; set; }
        public UnitClass Class { get; set; }
        public byte Position { get; set; }
    }

    public class TeamGenericSlotDetailModel : TeamGenericSlotStubModel, ISubItem
    {
        public bool Sub { get; set; }
    }


    public class TeamGenericSlotEditorModel : TeamGenericSlotDetailModel
    {
    }
}
