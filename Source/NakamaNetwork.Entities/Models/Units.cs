using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Units
    {
        public Units()
        {
            BoxUnits = new HashSet<BoxUnits>();
            Stages = new HashSet<Stages>();
            TeamUnits = new HashSet<TeamUnits>();
            UnitAliases = new HashSet<UnitAliases>();
            UnitEvolutionsFromUnit = new HashSet<UnitEvolutions>();
            UnitEvolutionsToUnit = new HashSet<UnitEvolutions>();
            UserProfiles = new HashSet<UserProfiles>();
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

        public virtual ICollection<BoxUnits> BoxUnits { get; set; }
        public virtual ICollection<Stages> Stages { get; set; }
        public virtual ICollection<TeamUnits> TeamUnits { get; set; }
        public virtual ICollection<UnitAliases> UnitAliases { get; set; }
        public virtual ICollection<UnitEvolutions> UnitEvolutionsFromUnit { get; set; }
        public virtual ICollection<UnitEvolutions> UnitEvolutionsToUnit { get; set; }
        public virtual ICollection<UserProfiles> UserProfiles { get; set; }
    }
}
