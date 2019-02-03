using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Team
    {
        public Team()
        {
            TeamBookmarks = new HashSet<TeamBookmark>();
            TeamComments = new HashSet<TeamComment>();
            TeamGenericSlots = new HashSet<TeamGenericSlot>();
            TeamReports = new HashSet<TeamReport>();
            TeamSockets = new HashSet<TeamSocket>();
            TeamUnits = new HashSet<TeamUnit>();
            TeamVideos = new HashSet<TeamVideo>();
            TeamVotes = new HashSet<TeamVote>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Guide { get; set; }
        public string Credits { get; set; }
        public int? StageId { get; set; }
        public int? InvasionId { get; set; }
        public int ShipId { get; set; }
        public string SubmittedById { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public string EditedById { get; set; }
        public DateTimeOffset EditedDate { get; set; }
        public int Version { get; set; }
        public bool Draft { get; set; }
        public bool Deleted { get; set; }

        public virtual UserProfile EditedBy { get; set; }
        public virtual Stage Invasion { get; set; }
        public virtual Ship Ship { get; set; }
        public virtual Stage Stage { get; set; }
        public virtual UserProfile SubmittedBy { get; set; }
        public virtual TeamCredit TeamCredits { get; set; }
        public virtual ICollection<TeamBookmark> TeamBookmarks { get; set; }
        public virtual ICollection<TeamComment> TeamComments { get; set; }
        public virtual ICollection<TeamGenericSlot> TeamGenericSlots { get; set; }
        public virtual ICollection<TeamReport> TeamReports { get; set; }
        public virtual ICollection<TeamSocket> TeamSockets { get; set; }
        public virtual ICollection<TeamUnit> TeamUnits { get; set; }
        public virtual ICollection<TeamVideo> TeamVideos { get; set; }
        public virtual ICollection<TeamVote> TeamVotes { get; set; }
    }
}
