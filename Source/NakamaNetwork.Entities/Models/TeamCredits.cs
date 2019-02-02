using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamCredits
    {
        public int TeamId { get; set; }
        public string Credit { get; set; }
        public short Type { get; set; }

        public virtual Teams Team { get; set; }
    }
}
