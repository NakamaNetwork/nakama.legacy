using System;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models
{
    public class CacheResults
    {
        public DateTimeOffset? Timestamp { get; set; }
        public object Items { get; set; }
    }
}
