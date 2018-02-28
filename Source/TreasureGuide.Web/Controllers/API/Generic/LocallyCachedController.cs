using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Entities.Interfaces;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public abstract class LocallyCachedController<TEntityKey, TEntity, TCacheModel> : Controller
        where TEntity : class, IIdItem<TEntityKey>, IEditedDateItem
        where TCacheModel : IIdItem<TEntityKey>, IEditedDateItem
    {
        private const double Timeout = 5;
        protected readonly TreasureEntities DbContext;
        protected readonly IMapper AutoMapper;
        protected readonly IThrottleService ThrottlingService;

        public bool Throttled { get; set; } = true;

        public LocallyCachedController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService)
        {
            DbContext = dbContext;
            AutoMapper = autoMapper;
            ThrottlingService = throttlingService;
        }

        [HttpGet]
        [ActionName("")]
        [Route("{date?}")]
        public async Task<IActionResult> Get(long? date = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request, RouteData, Timeout))
            {
                return StatusCode((int)HttpStatusCode.Conflict, ThrottleService.Message);
            }
            var entities = DbContext.Set<TEntity>().AsQueryable();
            if (date.HasValue)
            {
                var dateTime = date.Value.FromUnixEpochDate();
                entities = GetNewItems(entities, dateTime);
            }
            var results = await entities.ProjectTo<TCacheModel>(AutoMapper.ConfigurationProvider).ToListAsync();
            return Ok(results);
        }

        protected virtual IQueryable<TEntity> GetNewItems(IQueryable<TEntity> entities, DateTimeOffset date)
        {
            return entities.Where(x => x.EditedDate > date);
        }
    }
}
