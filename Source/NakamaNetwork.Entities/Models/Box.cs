using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Box
    {
        public Box()
        {
            BoxUnits = new HashSet<BoxUnit>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public decimal? FriendId { get; set; }
        public bool? Global { get; set; }
        public bool? Public { get; set; }

        public virtual UserProfile User { get; set; }
        public virtual ICollection<BoxUnit> BoxUnits { get; set; }
    }
}
