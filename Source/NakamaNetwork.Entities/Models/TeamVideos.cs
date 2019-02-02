using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamVideos
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string VideoLink { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public string UserId { get; set; }

        public virtual Teams Team { get; set; }
        public virtual UserProfiles User { get; set; }
    }
}
