using System.Collections.Generic;
using NakamaNetwork.Entities;
using NakamaNetwork.Entities.EnumTypes;

namespace TreasureGuide.Common.Models.UnitModels
{
    public class UnitSearchModel : SearchModel
    {
        public string Term { get; set; }
        public UnitClass? Classes { get; set; }
        public UnitType? Types { get; set; }
        public bool ForceClass { get; set; }
        public bool FreeToPlay { get; set; }

        public bool Global { get; set; }
        public bool MyBox { get; set; }
        public bool? Blacklist { get; set; }

        public IEnumerable<int> LimitTo { get; set; }
    }
}
