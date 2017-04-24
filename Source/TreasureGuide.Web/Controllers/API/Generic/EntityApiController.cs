using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        protected readonly IMapper AutoMapper;

        public EntityApiController(TreasureEntities dbContext, IMapper autoMapper)
        {
            DbContext = dbContext;
            AutoMapper = autoMapper;
        }

        protected override async Task<IActionResult> Get<TModel>(TKey? id = null)
        {
            var result = await PerformGet<TModel>(id);
            return result as IActionResult ?? Ok(result); ;
        }

        protected override async Task<IActionResult> Post(TEditorModel model, TKey? id = null)
        {
            var result = await PerformPost(model, id);
            return result as IActionResult ?? Ok(result); ;
        }

        protected override async Task<IActionResult> Delete(TKey? id = null)
        {
            var result = await PerformDelete(id);
            return result as IActionResult ?? Ok(result); ;
        }

        protected virtual async Task<object> PerformGet<TModel>(TKey? id = null)
        {
            var entities = FetchEntities(id);
            var transformed = typeof(TModel) == typeof(TEntity) ? entities.Cast<TModel>() : Project<TModel>(entities);
            if (id.HasValue)
            {
                var single = await transformed.SingleOrDefaultAsync();
                if (single != null)
                {
                    return single;
                }
                return NotFound(id);
            }
            return await transformed.ToListAsync();
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

        protected virtual async Task<object> PerformPost(TEditorModel model, TKey? id = null)
        {
            id = id ?? model.Id;
            if (id.HasValue)
            {
                var entities = FetchEntities(id);
                var single = entities.SingleOrDefault();
                if (single != null)
                {
                    return await CreateOrUpdate(model, single);
                }
            }
            return await CreateOrUpdate(model);
        }

        protected virtual async Task<object> PerformDelete(TKey? id)
        {
            if (id.HasValue)
            {
                var entities = FetchEntities(id);
                var target = entities.SingleOrDefault();
                return await Remove(target);
            }
            return BadRequest("No item specified.");
        }

        protected virtual async Task<object> CreateOrUpdate(TEditorModel model, TEntity entity = null)
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
            throw new NotImplementedException();
            DbContext.Set<TEntity>().Remove(single);
            await DbContext.SaveChangesAsync();
            return true;
        }

        protected virtual IQueryable<TModel> Project<TModel>(IQueryable<TEntity> entities)
        {
            return entities.ProjectTo<TModel>(AutoMapper.ConfigurationProvider);
        }

        protected virtual TEntity Create(TEditorModel model)
        {
            return AutoMapper.Map<TEntity>(model);
        }

        protected virtual TEntity Update(TEditorModel model, TEntity entity)
        {
            return AutoMapper.Map(model, entity);
        }

        protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> entities)
        {
            return entities;
        }

        protected virtual TEditorModel PreProcess(TEditorModel model)
        {
            return model;
        }

        protected virtual TEntity PostProcess(TEntity entity)
        {
            return entity;
        }
    }
}
