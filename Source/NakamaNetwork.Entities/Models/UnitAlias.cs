using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class UnitAlias
    {
        public int UnitId { get; set; }
        public string Name { get; set; }

        public virtual Unit Unit { get; set; }
    }
}
