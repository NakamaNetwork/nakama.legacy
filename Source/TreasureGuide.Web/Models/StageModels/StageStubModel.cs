using System;
using System.Collections.Generic;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models.StageModels
{
    public class StageStubModel : IIdItem<int>, IEditedDateItem
    {
        public int Id { get; set; }
        public int? UnitId { get; set; }
        public string Name { get; set; }
        public bool Global { get; set; }
        public StageType Type { get; set; }
        public DateTimeOffset? EditedDate { get; set; }

        public int TeamCount { get; set; }

        public IEnumerable<string> Aliases { get; set; }
    }
}
