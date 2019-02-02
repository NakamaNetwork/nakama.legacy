using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Ships
    {
        public Ships()
        {
            Teams = new HashSet<Teams>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool EventShip { get; set; }

        public virtual ICollection<Teams> Teams { get; set; }
    }
}
