using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamCommentVotes
    {
        public int TeamCommentId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public short Value { get; set; }

        public virtual TeamComments TeamComment { get; set; }
        public virtual UserProfiles User { get; set; }
    }
}
