using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Entities.Interfaces;
using TreasureGuide.Web.Models;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API.Generic
{
    public abstract class LocallyCachedController : Controller
    {
        private const double Timeout = 5;
        protected readonly TreasureEntities DbContext;
        protected readonly IThrottleService ThrottlingService;
        protected readonly CacheItemType Type;

        public bool Throttled { get; set; } = true;

        public LocallyCachedController(CacheItemType type, TreasureEntities dbContext, IThrottleService throttlingService)
        {
            Type = type;
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
            var found = DbContext.CacheSets.Where(x => x.Type == Type);
            if (date.HasValue)
            {
                var dateTime = date.Value.FromUnixEpochDate();
                found = found.Where(x => x.EditedDate > dateTime);
                var data = await found.SingleOrDefaultAsync();
                if (data != null)
                {
                    var result = new CacheResults
                    {
                        Timestamp = data.EditedDate,
                        Items = data.JSON
                    };
                    return Ok(result);
                }
            }
            return Ok(null);
        }
    }
}
