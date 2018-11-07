using System;

namespace TreasureGuide.Common.Models.NotificationModels
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public int EventType { get; set; }
        public int? EventId { get; set; }
        public string EventInfo { get; set; }
        public string ExtraInfo { get; set; }
        public string TriggerUserId { get; set; }
        public string TriggerUserName { get; set; }
        public DateTimeOffset ReceivedDate { get; set; }
    }
}