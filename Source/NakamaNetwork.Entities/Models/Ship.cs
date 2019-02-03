using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Ship
    {
        public Ship()
        {
            Teams = new HashSet<Team>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool EventShip { get; set; }

        public virtual ICollection<Team> Teams { get; set; }
    }
}
