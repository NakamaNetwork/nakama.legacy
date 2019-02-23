using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class StageAlias
    {
        public int StageId { get; set; }
        public string Name { get; set; }

        public virtual Stage Stage { get; set; }
    }
}
