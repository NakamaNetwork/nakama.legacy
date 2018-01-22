using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Interfaces;
using TreasureGuide.Web.Models;
using System.Data.Entity;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public abstract class SearchableApiController<TEntityKey, TEntity, TKey, TStubModel, TDetailModel, TEditorModel, TSearchModel> : EntityApiController<TEntityKey, TEntity, TKey, TStubModel, TDetailModel, TEditorModel>
        where TEntity : class, IIdItem<TEntityKey>
        where TEditorModel : IIdItem<TKey>
        where TSearchModel : SearchModel, new()
    {
        protected SearchableApiController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        [HttpGet]
        [ActionName("Search")]
        [Route("[action]")]
        public async Task<SearchResult<TStubModel>> Search(TSearchModel model)
        {
            model = model ?? new TSearchModel();
            model.PageSize = Math.Min(100, Math.Max(10, model.PageSize));
            var entities = FetchEntities();
            entities = (await PerformSearch(entities, model)).AsQueryable();
            var resultCount = await entities.CountAsync();
            entities = OrderSearchResults(entities, model);
            entities = entities.Skip(model.PageSize * (model.Page - 1)).Take(model.PageSize);
            var output = entities.ProjectTo<TStubModel>(AutoMapper.ConfigurationProvider);
            var results = await output.ToListAsync();
            return new SearchResult<TStubModel>
            {
                Results = results,
                TotalResults = resultCount
            };
        }

        protected abstract Task<IQueryable<TEntity>> PerformSearch(IQueryable<TEntity> results, TSearchModel model);

        protected virtual IQueryable<TEntity> OrderSearchResults(IQueryable<TEntity> results, TSearchModel model)
        {
            return results.OrderByDescending(x => x.Id);
        }
    }
}
