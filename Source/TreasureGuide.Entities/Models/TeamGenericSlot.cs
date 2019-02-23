using System;
using System.Collections.Generic;
using NakamaNetwork.Entities.EnumTypes;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamGenericSlot
    {
        public int TeamId { get; set; }
        public byte Position { get; set; }
        public UnitType Type { get; set; }
        public UnitClass Class { get; set; }
        public UnitRole Role { get; set; }
        public bool Sub { get; set; }

        public virtual Team Team { get; set; }
    }
}
