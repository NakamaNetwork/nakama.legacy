using System;

namespace NakamaNetwork.Entities.EnumTypes
{
    [Flags]
    public enum UnitClass : short
    {
        Unknown = 0,
        Fighter = 1,
        Slasher = 2,
        Striker = 4,
        Shooter = 8,
        FreeSpirit = 16,
        Driven = 32,
        Cerebral = 64,
        Powerhouse = 128,
        Evolver = 256,
        Booster = 512
    }
}
