using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Teams
    {
        public Teams()
        {
            TeamBookmarks = new HashSet<TeamBookmarks>();
            TeamComments = new HashSet<TeamComments>();
            TeamGenericSlots = new HashSet<TeamGenericSlots>();
            TeamReports = new HashSet<TeamReports>();
            TeamSockets = new HashSet<TeamSockets>();
            TeamUnits = new HashSet<TeamUnits>();
            TeamVideos = new HashSet<TeamVideos>();
            TeamVotes = new HashSet<TeamVotes>();
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

        public virtual UserProfiles EditedBy { get; set; }
        public virtual Stages Invasion { get; set; }
        public virtual Ships Ship { get; set; }
        public virtual Stages Stage { get; set; }
        public virtual UserProfiles SubmittedBy { get; set; }
        public virtual TeamCredits TeamCredits { get; set; }
        public virtual ICollection<TeamBookmarks> TeamBookmarks { get; set; }
        public virtual ICollection<TeamComments> TeamComments { get; set; }
        public virtual ICollection<TeamGenericSlots> TeamGenericSlots { get; set; }
        public virtual ICollection<TeamReports> TeamReports { get; set; }
        public virtual ICollection<TeamSockets> TeamSockets { get; set; }
        public virtual ICollection<TeamUnits> TeamUnits { get; set; }
        public virtual ICollection<TeamVideos> TeamVideos { get; set; }
        public virtual ICollection<TeamVotes> TeamVotes { get; set; }
    }
}
