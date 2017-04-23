using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;
using TreasureGuide.Web.Helpers;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public class EntityApiController<TKey, TEntity, TStubModel, TDetailModel, TEditorModel> : GenericApiController<TKey, TEntity, TStubModel, TDetailModel, TEditorModel>
        where TKey : struct
        where TEntity : class, IIdItem<TKey>
        where TEditorModel : IIdItem<TKey?>
    {
        protected readonly TreasureEntities DbContext;

        public EntityApiController(TreasureEntities dbContext)
        {
            DbContext = dbContext;
        }

        protected override IActionResult Get<TModel>(TKey? id = null)
        {
            return Do(() => PerformGet<TModel>(id));
        }

        protected override IActionResult Post(TEditorModel model, TKey? id = null)
        {
            return Do(() => PerformPost(model, id));
        }

        protected override IActionResult Delete(TKey? id = null)
        {
            return Do(() => PerformDelete(id));
        }

        protected virtual IActionResult Do(Func<object> function)
        {
            var result = function.Invoke();
            return (result as IActionResult) ?? Ok(result);
        }

        protected virtual object PerformGet<TModel>(TKey? id = null)
        {
            var entities = FetchEntities(id);
            var transformed = typeof(TModel) == typeof(TEntity) ? entities.Cast<TModel>() : Project<TModel>(entities);
            if (id.HasValue)
            {
                var single = transformed.SingleOrDefault();
                if (single != null)
                {
                    return single;
                }
                return NotFound(id);
            }
            return transformed;
        }

        protected virtual IQueryable<TEntity> FetchEntities(TKey? id = null)
        {
            var queryable = DbContext.Set<TEntity>().AsQueryable();
            if (id.HasValue)
            {
                queryable = queryable.FindId(id);
            }
            queryable = Filter(queryable);
            return queryable;
        }

        protected virtual object PerformPost(TEditorModel model, TKey? id = null)
        {
            if (id.HasValue)
            {
                var entities = FetchEntities(id);
                var single = entities.SingleOrDefault();
                if (single != null)
                {
                    return CreateOrUpdate(model, single);
                }
                return NotFound(id);
            }
            return BadRequest("No item specified.");
        }

        protected virtual object PerformDelete(TKey? id)
        {
            if (id.HasValue)
            {
                var entities = FetchEntities(id);
                var target = entities.SingleOrDefault();
                return Remove(target);
            }
            return BadRequest("No item specified.");
        }

        protected virtual async Task<object> CreateOrUpdate(TEditorModel model, TEntity entity)
        {
            model = PreProcess(model);
            var newItem = entity == null;
            if (newItem)
            {
                entity = Create(model);
                DbContext.Set<TEntity>().Add(entity);
            }
            else
            {
                entity = Update(model, entity);
            }
            entity = PostProcess(entity);
            await DbContext.SaveChangesAsync();
            return entity.Id;
        }

        protected virtual async Task<object> Remove(TEntity single)
        {
            DbContext.Set<TEntity>().Remove(single);
            await DbContext.SaveChangesAsync();
            return true;
        }

        protected virtual IQueryable<TModel> Project<TModel>(IQueryable<TEntity> entities)
        {
            return entities.Cast<TModel>();
        }

        protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> entities)
        {
            return entities;
        }

        protected virtual TEntity Create(TEditorModel model)
        {
            return model as TEntity;
        }

        protected virtual TEntity Update(TEditorModel model, TEntity entity)
        {
            return model as TEntity;
        }

        private TEditorModel PreProcess(TEditorModel model)
        {
            return model;
        }

        private TEntity PostProcess(TEntity entity)
        {
            return entity;
        }
    }
}
