using System;
using System.Collections.Generic;
using NakamaNetwork.Entities.EnumTypes;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamSocket
    {
        public int TeamId { get; set; }
        public SocketType SocketType { get; set; }
        public byte Level { get; set; }

        public virtual Team Team { get; set; }
    }
}
