using System;
using System.Collections.Generic;
using NakamaNetwork.Entities.EnumTypes;

namespace NakamaNetwork.Entities.Models
{
    public partial class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public NotificationEventType EventType { get; set; }
        public int? EventId { get; set; }
        public DateTimeOffset ReceivedDate { get; set; }

        public virtual UserProfile User { get; set; }
    }
}
