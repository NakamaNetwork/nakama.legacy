using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamCredit
    {
        public int TeamId { get; set; }
        public string Credit { get; set; }
        public short Type { get; set; }

        public virtual Team Team { get; set; }
    }
}
