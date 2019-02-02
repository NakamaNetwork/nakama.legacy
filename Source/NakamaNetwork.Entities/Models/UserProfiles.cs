using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class UserProfiles
    {
        public UserProfiles()
        {
            Boxes = new HashSet<Boxes>();
            Donations = new HashSet<Donations>();
            Notifications = new HashSet<Notifications>();
            TeamBookmarks = new HashSet<TeamBookmarks>();
            TeamCommentVotes = new HashSet<TeamCommentVotes>();
            TeamCommentsEditedBy = new HashSet<TeamComments>();
            TeamCommentsSubmittedBy = new HashSet<TeamComments>();
            TeamVideos = new HashSet<TeamVideos>();
            TeamVotes = new HashSet<TeamVotes>();
            TeamsEditedBy = new HashSet<Teams>();
            TeamsSubmittedBy = new HashSet<Teams>();
            UserPreferences = new HashSet<UserPreferences>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public decimal? FriendId { get; set; }
        public string Website { get; set; }
        public int? UnitId { get; set; }
        public bool? Global { get; set; }

        public virtual AspNetUsers IdNavigation { get; set; }
        public virtual Units Unit { get; set; }
        public virtual ICollection<Boxes> Boxes { get; set; }
        public virtual ICollection<Donations> Donations { get; set; }
        public virtual ICollection<Notifications> Notifications { get; set; }
        public virtual ICollection<TeamBookmarks> TeamBookmarks { get; set; }
        public virtual ICollection<TeamCommentVotes> TeamCommentVotes { get; set; }
        public virtual ICollection<TeamComments> TeamCommentsEditedBy { get; set; }
        public virtual ICollection<TeamComments> TeamCommentsSubmittedBy { get; set; }
        public virtual ICollection<TeamVideos> TeamVideos { get; set; }
        public virtual ICollection<TeamVotes> TeamVotes { get; set; }
        public virtual ICollection<Teams> TeamsEditedBy { get; set; }
        public virtual ICollection<Teams> TeamsSubmittedBy { get; set; }
        public virtual ICollection<UserPreferences> UserPreferences { get; set; }
    }
}
