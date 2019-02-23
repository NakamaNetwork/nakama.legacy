using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TreasureGuide.Web.Services.QueryService;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public class GenericDatabaseApiController<TEntity, TQueryService> : Controller
        where TEntity : class
        where TQueryService : IDatabaseQueryService<TEntity>
    {
        protected readonly TQueryService QueryService;

        public GenericDatabaseApiController(TQueryService queryService)
        {
            QueryService = queryService;
        }

        public async Task<IActionResult> Get(object id)
        {
            var found = QueryService.FindAsync(id);
            return Ok(found);
        }

        public async Task<IActionResult> Put(TEntity entity)
        {
            return await Post(entity);
        }

        public async Task<IActionResult> Post(TEntity entity)
        {
            var found = QueryService.CreateOrUpdate(entity);
            await QueryService.SaveChangesAsync();
            return Ok(found);
        }

        public async Task<IActionResult> Delete(TEntity entity)
        {
            var found = QueryService.Delete(entity);
            await QueryService.SaveChangesAsync();
            return Ok(found);
        }
    }
}
