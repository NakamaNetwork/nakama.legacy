using NakamaNetwork.Entities;
using NakamaNetwork.Entities.EnumTypes;

namespace TreasureGuide.Common.Models.StageModels
{
    public class StageSearchModel : SearchModel
    {
        public string Term { get; set; }
        public StageType? Type { get; set; }
        public bool Global { get; set; }
    }
}
