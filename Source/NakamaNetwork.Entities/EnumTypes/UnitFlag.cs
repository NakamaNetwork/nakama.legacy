using System;

namespace NakamaNetwork.Entities.EnumTypes
{
    [Flags]
    public enum UnitFlag : short
    {
        Unknown = 0,
        Global = 1,
        RareRecruit = 2,
        RareRecruitExclusive = 4,
        RareRecruitLimited = 8,
        Promotional = 16,
        Shop = 32
    }
}
