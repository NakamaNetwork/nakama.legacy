using System.Collections.Generic;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;
using TreasureGuide.Web.Models.TeamModels;

namespace TreasureGuide.Web.Models.StageModels
{
    public class StageStubModel : IIdItem<int>
    {
        public int Id { get; set; }
        public byte? Stamina { get; set; }
        public string Name { get; set; }
        public bool Global { get; set; }
        public StageType Type { get; set; }

        public int TeamCount { get; set; }
    }

    public class StageDetailModel : IIdItem<int>
    {
        public int Id { get; set; }
        public byte? Stamina { get; set; }
        public string Name { get; set; }
        public bool Global { get; set; }
        public StageType Type { get; set; }
        
        public IEnumerable<TeamStubModel> Teams { get; set; }
    }

    public class StageEditorModel : IIdItem<int?>
    {
        public int? Id { get; set; }
        public byte? Stamina { get; set; }
        public string Name { get; set; }
        public bool Global { get; set; }
        public StageType Type { get; set; }
    }
}
