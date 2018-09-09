using System.Collections.Generic;

namespace TreasureGuide.Common.Models.ScheduleModels
{
    public class ScheduleModel
    {
        public IEnumerable<int> Global { get; set; }
        public IEnumerable<int> Japan { get; set; }
    }
}
