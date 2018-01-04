using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Models
{
    public class IdResponse<T> : IIdItem<T>
    {
        public T Id { get; set; }
    }
}
