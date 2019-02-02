using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamVotes
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public short Value { get; set; }

        public virtual Teams Team { get; set; }
        public virtual UserProfiles User { get; set; }
    }
}
