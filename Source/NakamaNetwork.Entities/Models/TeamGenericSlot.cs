using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamGenericSlot
    {
        public int TeamId { get; set; }
        public byte Position { get; set; }
        public short Type { get; set; }
        public short Class { get; set; }
        public short Role { get; set; }
        public bool Sub { get; set; }

        public virtual Team Team { get; set; }
    }
}
