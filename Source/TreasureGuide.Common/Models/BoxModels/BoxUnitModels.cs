using TreasureGuide.Entities;

namespace TreasureGuide.Common.Models.BoxModels
{
    public class BoxUnitStubModel
    {
        public int UnitId { get; set; }
        public IndividualUnitFlags? Flags { get; set; }
    }

    public class BoxUnitDetailModel : BoxUnitStubModel
    {
    }

    public class BoxUnitEditorModel
    {
        public int UnitId { get; set; }
        public int? Flags { get; set; }
    }
}
