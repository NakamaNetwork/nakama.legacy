using System;
using System.Collections.Generic;

namespace NakamaNetwork.Entities.Models
{
    public partial class Notifications
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int EventType { get; set; }
        public int? EventId { get; set; }
        public DateTimeOffset ReceivedDate { get; set; }

        public virtual UserProfiles User { get; set; }
    }
}
