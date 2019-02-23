using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamComment
    {
        public TeamComment()
        {
            InverseParent = new HashSet<TeamComment>();
            TeamCommentVotes = new HashSet<TeamCommentVote>();
        }

        public int Id { get; set; }
        public int TeamId { get; set; }
        public int? ParentId { get; set; }
        public string Text { get; set; }
        public bool Deleted { get; set; }
        public bool Reported { get; set; }
        public string SubmittedById { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public string EditedById { get; set; }
        public DateTimeOffset EditedDate { get; set; }
        public int Version { get; set; }

        public virtual UserProfile EditedBy { get; set; }
        public virtual TeamComment Parent { get; set; }
        public virtual UserProfile SubmittedBy { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<TeamComment> InverseParent { get; set; }
        public virtual ICollection<TeamCommentVote> TeamCommentVotes { get; set; }
    }
}
