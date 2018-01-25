using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.UnitModels
{
    public abstract class UnitModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public UnitClass Class { get; set; }
        public UnitType Type { get; set; }
        public UnitFlag Flags { get; set; }
    }

    public class UnitStubModel : UnitModel
    {
    }

    public class UnitDetailModel : UnitModel
    {
    }

    public class UnitEditorModel : IIdItem<int?>
    {
        public int? Id { get; set; }
    }
}
