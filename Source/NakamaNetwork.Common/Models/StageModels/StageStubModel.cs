using System;
using System.Collections.Generic;
using NakamaNetwork.Entities;
using NakamaNetwork.Entities.Interfaces;
using NakamaNetwork.Entities.EnumTypes;

namespace TreasureGuide.Common.Models.StageModels
{
    public class StageStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public int? UnitId { get; set; }
        public string Name { get; set; }
        public bool Global { get; set; }
        public StageType Type { get; set; }

        public IEnumerable<string> Aliases { get; set; }
    }
}
