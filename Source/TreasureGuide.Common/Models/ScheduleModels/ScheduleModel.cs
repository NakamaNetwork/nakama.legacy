using System.Collections.Generic;

namespace TreasureGuide.Common.Models.ScheduleModels
{
    public class ScheduleModel
    {
        public ScheduleSubModel Live { get; set; }
        public ScheduleSubModel Upcoming { get; set; }
    }

    public class ScheduleSubModel
    {

        public IEnumerable<int> Global { get; set; }
        public IEnumerable<int> Japan { get; set; }
    }
}
