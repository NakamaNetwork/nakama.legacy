using System;
using System.Collections.Generic;
using NakamaNetwork.Entities.EnumTypes;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamUnit
    {
        public int TeamId { get; set; }
        public int UnitId { get; set; }
        public byte Position { get; set; }
        public IndividualUnitFlags? Flags { get; set; }
        public bool Sub { get; set; }

        public virtual Team Team { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
