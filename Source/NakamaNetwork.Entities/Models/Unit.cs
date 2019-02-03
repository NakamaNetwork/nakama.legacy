using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Unit
    {
        public Unit()
        {
            BoxUnits = new HashSet<BoxUnit>();
            Stages = new HashSet<Stage>();
            TeamUnits = new HashSet<TeamUnit>();
            UnitAliases = new HashSet<UnitAlias>();
            UnitEvolutionsFromUnit = new HashSet<UnitEvolution>();
            UnitEvolutionsToUnit = new HashSet<UnitEvolution>();
            UserProfiles = new HashSet<UserProfile>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public short Type { get; set; }
        public short Class { get; set; }
        public decimal? Stars { get; set; }
        public byte? Cost { get; set; }
        public byte? Combo { get; set; }
        public byte? Sockets { get; set; }
        public byte? MaxLevel { get; set; }
        public int? ExptoMax { get; set; }
        public short? MinHp { get; set; }
        public short? MinAtk { get; set; }
        public short? MinRcv { get; set; }
        public short? MaxHp { get; set; }
        public short? MaxAtk { get; set; }
        public short? MaxRcv { get; set; }
        public decimal? GrowthRate { get; set; }
        public short Flags { get; set; }

        public virtual ICollection<BoxUnit> BoxUnits { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
        public virtual ICollection<TeamUnit> TeamUnits { get; set; }
        public virtual ICollection<UnitAlias> UnitAliases { get; set; }
        public virtual ICollection<UnitEvolution> UnitEvolutionsFromUnit { get; set; }
        public virtual ICollection<UnitEvolution> UnitEvolutionsToUnit { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
