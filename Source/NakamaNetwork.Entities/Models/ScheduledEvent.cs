using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class ScheduledEvent
    {
        public int StageId { get; set; }
        public bool Global { get; set; }
        public bool Source { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public virtual Stage Stage { get; set; }
    }
}
