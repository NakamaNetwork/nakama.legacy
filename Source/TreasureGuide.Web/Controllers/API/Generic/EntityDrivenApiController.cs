using TreasureGuide.Web.Services.API.Generic;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public abstract class EntityDrivenApiController<TKey, TEntity, TStubModel, TDetailModel, TEditorModel> : ServiceDrivenApiController<TKey, TStubModel, TDetailModel, TEditorModel>
        where TKey : struct
    {
        protected EntityDrivenApiController(IEntityDataService<TKey, TEntity> dataService) : base(dataService)
        {
        }
    }
}
