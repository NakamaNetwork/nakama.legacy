using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Stages
    {
        public Stages()
        {
            ScheduledEvents = new HashSet<ScheduledEvents>();
            StageAliases = new HashSet<StageAliases>();
            TeamsInvasion = new HashSet<Teams>();
            TeamsStage = new HashSet<Teams>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte? Stamina { get; set; }
        public int? UnitId { get; set; }
        public byte Type { get; set; }
        public bool? Global { get; set; }

        public virtual Units Unit { get; set; }
        public virtual ICollection<ScheduledEvents> ScheduledEvents { get; set; }
        public virtual ICollection<StageAliases> StageAliases { get; set; }
        public virtual ICollection<Teams> TeamsInvasion { get; set; }
        public virtual ICollection<Teams> TeamsStage { get; set; }
    }
}
