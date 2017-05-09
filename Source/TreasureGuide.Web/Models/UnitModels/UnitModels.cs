using System.Collections.Generic;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.UnitModels
{
    public abstract class UnitModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public IEnumerable<UnitClassType> UnitClasses { get; set; }
    }

    public class UnitStubModel : UnitModel
    {
        public bool Global { get; set; }
    }

    public class UnitDetailModel : UnitModel
    {
        public IEnumerable<UnitFlagType> UnitFlags { get; set; }
    }

    public class UnitEditorModel : IIdItem<int?>
    {
        public int? Id { get; set; }
    }
}
