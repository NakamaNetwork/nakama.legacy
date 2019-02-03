using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class UnitEvolution
    {
        public int FromUnitId { get; set; }
        public int ToUnitId { get; set; }

        public virtual Unit FromUnit { get; set; }
        public virtual Unit ToUnit { get; set; }
    }
}
