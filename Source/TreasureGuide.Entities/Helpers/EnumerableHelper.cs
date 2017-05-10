using System.Collections.Generic;

namespace TreasureGuide.Entities.Helpers
{
    public static class EnumerableHelper
    {
        public static readonly IEnumerable<UnitFlag> PayToPlay = new[]
        {
            UnitFlag.RareRecruitExclusive,
            UnitFlag.RareRecruitLimited
        };
    }
}
