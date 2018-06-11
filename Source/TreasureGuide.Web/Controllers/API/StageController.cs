using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.ScheduleModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/stage")]
    public class StageController : LocallyCachedController
    {
        public StageController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(CacheItemType.Stage, dbContext, throttlingService)
        {
        }

        [HttpGet]
        [ActionName("Schedule")]
        [Route("[action]")]
        public async Task<IActionResult> Schedule()
        {
            var now = DateTimeOffset.Now;
            var later = now.AddDays(7);
            var live = await DbContext.ScheduledEvents
                .Where(x => x.StartDate <= now && x.EndDate > now)
                .GroupBy(x => x.Global)
                .ToDictionaryAsync(x => x.Key, x => x.Select(y => y.StageId).Distinct());
            var upcoming = await DbContext.ScheduledEvents
                .Where(x => x.StartDate > now && x.StartDate <= later)
                .GroupBy(x => x.Global)
                .ToDictionaryAsync(x => x.Key, x => x.Select(y => y.StageId).Distinct());

            var results = new ScheduleModel
            {
                Live = new ScheduleSubModel
                {
                    Global = live.SafeGet(true),
                    Japan = live.SafeGet(false)
                },
                Upcoming = new ScheduleSubModel
                {
                    Global = upcoming.SafeGet(true),
                    Japan = upcoming.SafeGet(false)
                },
            };
            return Ok(results);
        }
    }
}
