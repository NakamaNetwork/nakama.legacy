//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TreasureGuide.Entities
{
    using System;
    
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
