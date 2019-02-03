using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamCommentVote
    {
        public int TeamCommentId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public short Value { get; set; }

        public virtual TeamComment TeamComment { get; set; }
        public virtual UserProfile User { get; set; }
    }
}
