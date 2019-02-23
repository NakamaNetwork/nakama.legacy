using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            Boxes = new HashSet<Box>();
            Donations = new HashSet<Donation>();
            Notifications = new HashSet<Notification>();
            TeamBookmarks = new HashSet<TeamBookmark>();
            TeamCommentVotes = new HashSet<TeamCommentVote>();
            TeamCommentsEditedBy = new HashSet<TeamComment>();
            TeamCommentsSubmittedBy = new HashSet<TeamComment>();
            TeamVideos = new HashSet<TeamVideo>();
            TeamVotes = new HashSet<TeamVote>();
            TeamsEditedBy = new HashSet<Team>();
            TeamsSubmittedBy = new HashSet<Team>();
            UserPreferences = new HashSet<UserPreference>();
            UserRoles = new HashSet<UserRole>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public decimal? FriendId { get; set; }
        public string Website { get; set; }
        public int? UnitId { get; set; }
        public bool? Global { get; set; }

        public virtual Unit Unit { get; set; }
        public virtual ICollection<Box> Boxes { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<TeamBookmark> TeamBookmarks { get; set; }
        public virtual ICollection<TeamCommentVote> TeamCommentVotes { get; set; }
        public virtual ICollection<TeamComment> TeamCommentsEditedBy { get; set; }
        public virtual ICollection<TeamComment> TeamCommentsSubmittedBy { get; set; }
        public virtual ICollection<TeamVideo> TeamVideos { get; set; }
        public virtual ICollection<TeamVote> TeamVotes { get; set; }
        public virtual ICollection<Team> TeamsEditedBy { get; set; }
        public virtual ICollection<Team> TeamsSubmittedBy { get; set; }
        public virtual ICollection<UserPreference> UserPreferences { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
