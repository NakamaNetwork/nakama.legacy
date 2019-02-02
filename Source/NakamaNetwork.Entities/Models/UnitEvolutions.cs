using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class UnitEvolutions
    {
        public int FromUnitId { get; set; }
        public int ToUnitId { get; set; }

        public virtual Units FromUnit { get; set; }
        public virtual Units ToUnit { get; set; }
    }
}
