using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TreasureGuide.Common.Models;
using TreasureGuide.Web.Services.QueryService;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public class SearchableDatabaseApiController<TEntity, TSearchModel, TQueryService> : GenericDatabaseApiController<TEntity, TQueryService>
        where TEntity : class
        where TSearchModel : SearchModel
        where TQueryService : ISearchableDatabaseQueryService<TEntity, TSearchModel>
    {
        public SearchableDatabaseApiController(TQueryService queryService) : base(queryService)
        {
        }

        public async Task<IActionResult> Search(TSearchModel searchModel)
        {
            var results = QueryService.Search(searchModel);
            var converted = await results.ToListAsync();
            return Ok(converted);
        }
    }
}
