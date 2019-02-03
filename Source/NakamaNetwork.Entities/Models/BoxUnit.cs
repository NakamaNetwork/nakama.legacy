using System;
using System.Collections.Generic;
using NakamaNetwork.Entities.EnumTypes;

namespace NakamaNetwork.Entities.Models
{
    public partial class BoxUnit
    {
        public int BoxId { get; set; }
        public int UnitId { get; set; }
        public IndividualUnitFlags? Flags { get; set; }

        public virtual Box Box { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
