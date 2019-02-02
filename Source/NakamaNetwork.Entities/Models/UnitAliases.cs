using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class UnitAliases
    {
        public int UnitId { get; set; }
        public string Name { get; set; }

        public virtual Units Unit { get; set; }
    }
}
