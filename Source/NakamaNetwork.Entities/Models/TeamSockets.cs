using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class TeamSockets
    {
        public int TeamId { get; set; }
        public byte SocketType { get; set; }
        public byte Level { get; set; }

        public virtual Teams Team { get; set; }
    }
}
