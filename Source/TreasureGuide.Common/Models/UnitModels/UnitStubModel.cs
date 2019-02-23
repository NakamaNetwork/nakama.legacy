using NakamaNetwork.Entities.EnumTypes;
using NakamaNetwork.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace TreasureGuide.Common.Models.UnitModels
{
    public class UnitStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxLevel { get; set; }
        public decimal? Stars { get; set; }
        public int Cost { get; set; }

        public UnitClass Class { get; set; }
        public UnitType Type { get; set; }
        public UnitFlag Flags { get; set; }

        public IEnumerable<string> Aliases { get; set; }
        public IEnumerable<int> EvolvesTo { get; set; }
        public IEnumerable<int> EvolvesFrom { get; set; }
    }
}
