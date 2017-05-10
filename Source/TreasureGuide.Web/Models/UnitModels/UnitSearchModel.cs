using System.Collections.Generic;
using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.UnitModels
{
    public class UnitSearchModel : SearchModel
    {
        public string Term { get; set; }
        public IEnumerable<UnitClass> Classes { get; set; }
        public IEnumerable<UnitType> Types { get; set; }
        public bool ForceTypes { get; set; }
        public bool FreeToPlay { get; set; }

        public bool Global { get; set; }
        public bool MyBox { get; set; }
    }
}
