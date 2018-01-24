using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.TeamModels
{
    public class TeamGenericSlotStubModel
    {
        public UnitRole? Role { get; set; }
        public UnitType? Type { get; set; }
        public UnitClass? Class { get; set; }
        public byte Position { get; set; }
    }

    public class TeamGenericSlotDetailModel : TeamGenericSlotStubModel
    {
        public byte? Combo { get; set; }
    }


    public class TeamGenericSlotEditorModel : TeamGenericSlotDetailModel
    {
    }
}
