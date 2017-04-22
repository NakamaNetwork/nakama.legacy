using System.Collections.Generic;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.UnitModels
{
    public abstract class UnitModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UnitType UnitType { get; set; }
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
}
