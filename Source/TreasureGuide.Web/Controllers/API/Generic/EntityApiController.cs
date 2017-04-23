using System;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public class EntityApiController<TKey, TEntity, TStubModel, TDetailModel, TEditorModel> : GenericApiController<TKey, TEntity, TStubModel, TDetailModel, TEditorModel>
        where TKey : struct
        where TEntity : IIdItem<TKey>
        where TEditorModel : IIdItem<TKey?>
    {
        protected readonly TreasureEntities DbContext;

        public EntityApiController(TreasureEntities dbContext)
        {
            DbContext = dbContext;
        }

        protected override IActionResult Get<TModel>(TKey? id = null)
        {
            return Do(() => PerformGet(id));
        }


        protected override IActionResult Post(TEditorModel model, TKey? id = null)
        {
            return Do(() => PerformPost(model, id));
        }

        protected override IActionResult Delete(TKey? id = null)
        {
            return Do(() => PerformDelete(id));
        }

        protected object PerformGet(TKey? id)
        {
            throw new NotImplementedException();
        }

        protected object PerformPost(TEditorModel model, TKey? id = null)
        {
            throw new NotImplementedException();
        }

        protected object PerformDelete(TKey? id)
        {
            throw new NotImplementedException();
        }

        protected virtual IActionResult Do(Func<object> function)
        {
            var result = function.Invoke();
            return (result as IActionResult) ?? Ok(result);
        }
    }
}
