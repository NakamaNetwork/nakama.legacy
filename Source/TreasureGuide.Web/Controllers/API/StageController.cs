using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NakamaNetwork.Entities;
using NakamaNetwork.Entities.Helpers;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Common.Models.ScheduleModels;
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
            var later = now.AddDays(5);
            var events = await DbContext.ScheduledEvents
                .Where(x => (x.StartDate <= now && x.EndDate > now) || (x.StartDate >= now && x.StartDate <= later))
                .GroupBy(x => x.Global)
                .ToDictionaryAsync(x => x.Key, x => x.Select(y => y.StageId).Distinct());

            var results = new ScheduleModel
            {
                Global = events.SafeGet(true),
                Japan = events.SafeGet(false)
            };
            return Ok(results);
        }
    }
}
