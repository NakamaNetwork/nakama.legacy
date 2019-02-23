using System;

namespace NakamaNetwork.Entities.EnumTypes
{
    [Flags]
    public enum UnitType : short
    {
        Unknown = 0,
        STR = 1,
        DEX = 2,
        QCK = 4,
        INT = 8,
        PSY = 16
    }
}
