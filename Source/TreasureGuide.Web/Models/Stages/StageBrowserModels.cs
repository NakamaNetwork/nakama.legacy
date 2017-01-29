using System.Collections.Generic;

namespace TreasureGuide.Web.Models.Stages
{
    public class StageTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<StageStubModel> Stages { get; set; }
    }

    public class StageStubModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Global { get; set; }
        public IEnumerable<StageDifficultyStubModel> Difficulties { get; set; }
    }

    public class StageDifficultyStubModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Stamina { get; set; }
        public bool Global { get; set; }
        public int Teams { get; set; }
    }
}