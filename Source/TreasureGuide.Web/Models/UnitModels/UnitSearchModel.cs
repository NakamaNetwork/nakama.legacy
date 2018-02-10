using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.UnitModels
{
    public class UnitSearchModel : SearchModel
    {
        public string Term { get; set; }
        public UnitClass? Classes { get; set; }
        public UnitType? Types { get; set; }
        public bool ForceClass { get; set; }
        public bool FreeToPlay { get; set; }

        public bool Global { get; set; }
        public int? Box { get; set; }
    }
}
