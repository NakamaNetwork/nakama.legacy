using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Boxes
    {
        public Boxes()
        {
            BoxUnits = new HashSet<BoxUnits>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public decimal? FriendId { get; set; }
        public bool? Global { get; set; }
        public bool? Public { get; set; }

        public virtual UserProfiles User { get; set; }
        public virtual ICollection<BoxUnits> BoxUnits { get; set; }
    }
}
