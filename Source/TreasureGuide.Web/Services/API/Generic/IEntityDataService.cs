namespace TreasureGuide.Web.Services.API.Generic
{
    public interface IEntityDataService<TKey, TEntity> : IDataService<TKey>
        where TKey : struct
    {
    }
}
