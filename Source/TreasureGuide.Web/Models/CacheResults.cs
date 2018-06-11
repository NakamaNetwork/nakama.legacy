using System;
using System.Collections.Generic;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models
{
    public class CacheResults<TId, TItem> where TItem : IIdItem<TId>
    {
        public DateTimeOffset? Timestamp { get; set; }
        public IEnumerable<TItem> Items { get; set; }
    }
}
