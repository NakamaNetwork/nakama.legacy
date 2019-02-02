using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamUnits
    {
        public int TeamId { get; set; }
        public int UnitId { get; set; }
        public byte Position { get; set; }
        public int? Flags { get; set; }
        public bool Sub { get; set; }

        public virtual Teams Team { get; set; }
        public virtual Units Unit { get; set; }
    }
}
