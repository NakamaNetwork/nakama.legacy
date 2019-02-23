using NakamaNetwork.Entities.Interfaces;

namespace TreasureGuide.Common.Models
{
    public class IdResponse<T> : IIdItem<T>
    {
        public T Id { get; set; }
    }
}
