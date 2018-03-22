using System;
using System.Collections.Generic;
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
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public abstract class LocallyCachedController<TEntityKey, TEntity, TCacheModel> : Controller
        where TEntity : class, IIdItem<TEntityKey>, IEditedDateItem
        where TCacheModel : IIdItem<TEntityKey>
    {
        private const double Timeout = 5;
        protected readonly TreasureEntities DbContext;
        protected readonly IMapper AutoMapper;
        protected readonly IThrottleService ThrottlingService;
        protected readonly DeletedItemType Type;

        public bool Throttled { get; set; } = true;

        public LocallyCachedController(DeletedItemType type, TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService)
        {
            Type = type;
            DbContext = dbContext;
            AutoMapper = autoMapper;
            ThrottlingService = throttlingService;
        }

        [HttpGet]
        [ActionName("")]
        [Route("{date?}")]
        public async Task<IActionResult> Get(long? date = null)
        {
            if (Throttled && !ThrottlingService.CanAccess(User, Request, Request.Path, Timeout))
            {
                return StatusCode(429, ThrottleService.Message);
            }
            var reset = false;
            var entities = DbContext.Set<TEntity>().AsQueryable();
            IEnumerable<int> deleted = null;
            if (date.HasValue)
            {
                var dateTime = date.Value.FromUnixEpochDate();
                reset = await DbContext.DeletedItems.AnyAsync(x => x.Type == DeletedItemType.FullReset && x.EditedDate > dateTime);
                if (!reset)
                {
                    entities = GetNewItems(entities, dateTime);
                    deleted = GetDeletedItems(dateTime);
                }
            }
            var timestamp = GetTimeStamp(entities);
            var results = await entities.ProjectTo<TCacheModel>(AutoMapper.ConfigurationProvider).ToListAsync();
            if (results.Any())
            {
                var result = new CacheResults<TEntityKey, TCacheModel>
                {
                    Items = results,
                    Timestamp = timestamp,
                    Deleted = deleted,
                    Reset = reset
                };
                return Ok(result);
            }
            return Ok(null);
        }

        protected DateTimeOffset? GetTimeStamp(IQueryable<TEntity> entities)
        {
            var deleted = DbContext.DeletedItems.Where(y => y.Type == Type || y.Type == DeletedItemType.FullReset).Select(x => x.EditedDate);
            return GetTimeStamps(entities).Where(x => x != null).Select(x => x.Value).Concat(deleted).Max(x => x);
        }

        protected virtual IQueryable<DateTimeOffset?> GetTimeStamps(IQueryable<TEntity> entities)
        {
            return entities.Select(x => x.EditedDate);
        }

        private IEnumerable<int> GetDeletedItems(DateTimeOffset date)
        {
            return DbContext.DeletedItems.Where(x => x.Type == Type && x.EditedDate > date).Select(x => x.Id);
        }

        protected virtual IQueryable<TEntity> GetNewItems(IQueryable<TEntity> entities, DateTimeOffset date)
        {
            return entities.Where(x => x.EditedDate > date);
        }
    }
}
