using System.Collections.Generic;
using NakamaNetwork.Entities;
using NakamaNetwork.Entities.EnumTypes;

namespace TreasureGuide.Common.Models.BoxModels
{
    public class BoxUpdateModel
    {
        public int Id { get; set; }
        public IEnumerable<int> Added { get; set; }
        public IEnumerable<int> Removed { get; set; }
        public IEnumerable<BoxUnitUpdateModel> Updated { get; set; }
    }

    public class BoxUnitUpdateModel
    {
        public int Id { get; set; }
        public IndividualUnitFlags? Flags { get; set; }
    }
}
