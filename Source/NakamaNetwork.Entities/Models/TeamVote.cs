using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamVote
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public short Value { get; set; }

        public virtual Team Team { get; set; }
        public virtual UserProfile User { get; set; }
    }
}
