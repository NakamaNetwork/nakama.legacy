using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamReport
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Reason { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public DateTimeOffset? AcknowledgedDate { get; set; }

        public virtual Team Team { get; set; }
    }
}
