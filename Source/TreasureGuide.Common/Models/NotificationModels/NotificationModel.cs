using System;
using TreasureGuide.Entities;

namespace TreasureGuide.Common.Models.NotificationModels
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public NotificationEventType EventType { get; set; }
        public int? EventId { get; set; }
        public string TriggerUserId { get; set; }
        public string TriggerName { get; set; }
        public DateTimeOffset ReceivedDate { get; set; }
    }
}