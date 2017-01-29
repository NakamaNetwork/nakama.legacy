using TreasureGuide.Entities;

namespace TreasureGuide.Web.Models.Stages
{
    public class SaveStageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StageType Type { get; set; }
        public bool Global { get; set; }
    }
}