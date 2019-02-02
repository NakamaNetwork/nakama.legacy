using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class BoxUnits
    {
        public int BoxId { get; set; }
        public int UnitId { get; set; }
        public int? Flags { get; set; }

        public virtual Boxes Box { get; set; }
        public virtual Units Unit { get; set; }
    }
}
