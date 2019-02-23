using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamVideo
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string VideoLink { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public string UserId { get; set; }

        public virtual Team Team { get; set; }
        public virtual UserProfile User { get; set; }
    }
}
