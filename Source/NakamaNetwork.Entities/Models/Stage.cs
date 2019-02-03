using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Stage
    {
        public Stage()
        {
            ScheduledEvents = new HashSet<ScheduledEvent>();
            StageAliases = new HashSet<StageAlias>();
            TeamsInvasion = new HashSet<Team>();
            TeamsStage = new HashSet<Team>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte? Stamina { get; set; }
        public int? UnitId { get; set; }
        public byte Type { get; set; }
        public bool? Global { get; set; }

        public virtual Unit Unit { get; set; }
        public virtual ICollection<ScheduledEvent> ScheduledEvents { get; set; }
        public virtual ICollection<StageAlias> StageAliases { get; set; }
        public virtual ICollection<Team> TeamsInvasion { get; set; }
        public virtual ICollection<Team> TeamsStage { get; set; }
    }
}
