using System;
using System.Collections.Generic;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.UnitModels
{
    public class UnitStubModel : IIdItem<int>, IEditedDateItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxLevel { get; set; }
        public decimal? Stars { get; set; }

        public UnitClass Class { get; set; }
        public UnitType Type { get; set; }
        public UnitFlag Flags { get; set; }

        public IEnumerable<string> Aliases { get; set; }
        public IEnumerable<int> EvolvesTo { get; set; }
        public IEnumerable<int> EvolvesFrom { get; set; }

        public DateTimeOffset EditedDate { get; set; }
    }
}
