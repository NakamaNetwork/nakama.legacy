using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamBookmarks
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }

        public virtual Teams Team { get; set; }
        public virtual UserProfiles User { get; set; }
    }
}
