using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class StageAliases
    {
        public int StageId { get; set; }
        public string Name { get; set; }

        public virtual Stages Stage { get; set; }
    }
}
