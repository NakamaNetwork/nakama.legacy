using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.BoxModels
{
    public class BoxUnitStubModel
    {
        public string Name { get; set; }
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
