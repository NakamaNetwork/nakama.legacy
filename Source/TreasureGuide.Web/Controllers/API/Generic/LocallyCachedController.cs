using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NakamaNetwork.Entities.Models;
using NakamaNetwork.Entities.Helpers;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Services;
using NakamaNetwork.Entities.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public abstract class LocallyCachedController<TEntity> : Controller
        where TEntity : class
    {
        private const double Timeout = 5;
        protected readonly NakamaNetworkContext DbContext;
        protected readonly IThrottleService ThrottlingService;

        public bool Throttled { get; set; } = true;

        public LocallyCachedController(NakamaNetworkContext dbContext, IThrottleService throttlingService)
        {
            DbContext = dbContext;
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
            var found = DbContext.Set<TEntity>().AsQueryable();
            DateTimeOffset dateTime;
            if (date.HasValue)
            {
                dateTime = date.Value.FromUnixEpochDate();
            }
            else
            {
                dateTime = DateTimeOffset.Now;
            }
            var result = new CacheResults
            {
                Timestamp = dateTime,
                Items = await found.ToListAsync()
            };
            return Ok(result);
        }
    }
}
