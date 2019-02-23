using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using NakamaNetwork.Entities.Models;
using NakamaNetwork.Entities.Interfaces;
using TreasureGuide.Common.Models;
using System.Data.Entity;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public abstract class SearchableApiController<TEntityKey, TEntity, TKey, TStubModel, TDetailModel, TEditorModel, TSearchModel> : EntityApiController<TEntityKey, TEntity, TKey, TStubModel, TDetailModel, TEditorModel>
        where TEntity : class, IIdItem<TEntityKey>
        where TEditorModel : IIdItem<TKey>
        where TSearchModel : SearchModel, new()
    {
        protected SearchableApiController(NakamaNetworkContext dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override async Task<IActionResult> Stub(TKey id = default(TKey))
        {
            return await Get<TStubModel>(id, true);
        }

        [HttpGet]
        [ActionName("Search")]
        [Route("[action]")]
        public async Task<SearchResult<TStubModel>> Search(TSearchModel model)
        {
            return await Search<TStubModel>(model);
        }

        protected async Task<SearchResult<TOutput>> Search<TOutput>(TSearchModel model)
        {
            model = model ?? new TSearchModel();
            model.PageSize = Math.Min(100, Math.Max(5, model.PageSize));
            var entities = FetchEntities();
            entities = (await PerformSearch(entities, model)).AsQueryable();
            var resultCount = await entities.CountAsync();
            entities = OrderSearchResults(entities, model);
            entities = entities.Skip(model.PageSize * (model.Page - 1)).Take(model.PageSize);
            var output = Project<TOutput>(entities);
            var results = await output.ToListAsync();
            return new SearchResult<TOutput>
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
