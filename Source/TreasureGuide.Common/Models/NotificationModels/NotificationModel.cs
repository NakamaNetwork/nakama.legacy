using System;

namespace TreasureGuide.Common.Models.NotificationModels
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public int? EventId { get; set; }
        public int EventType { get; set; }
        public DateTimeOffset ReceivedDate { get; set; }
    }
}