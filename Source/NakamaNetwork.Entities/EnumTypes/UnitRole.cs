using System;

namespace NakamaNetwork.Entities.EnumTypes
{
    [Flags]
    public enum UnitRole : short
    {
        Unknown = 0,
        Beatstick = 1,
        DamageReducer = 2,
        DefenseReducer = 4,
        Delayer = 8,
        AttackBooster = 16,
        OrbBooster = 32,
        FixedDamage = 64,
        HealthCutter = 128,
        OrbShuffler = 256,
        Healer = 512,
        Zombie = 1024,
        Nuker = 2048
    }
}
