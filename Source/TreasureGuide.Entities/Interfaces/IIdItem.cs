namespace TreasureGuide.Entities.Interfaces
{
    public interface IIdItem<TKey>
    {
        TKey Id { get; set; }
    }
}
