using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamComments
    {
        public TeamComments()
        {
            InverseParent = new HashSet<TeamComments>();
            TeamCommentVotes = new HashSet<TeamCommentVotes>();
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

        public virtual UserProfiles EditedBy { get; set; }
        public virtual TeamComments Parent { get; set; }
        public virtual UserProfiles SubmittedBy { get; set; }
        public virtual Teams Team { get; set; }
        public virtual ICollection<TeamComments> InverseParent { get; set; }
        public virtual ICollection<TeamCommentVotes> TeamCommentVotes { get; set; }
    }
}
