using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.ScheduleModels;
using TreasureGuide.Web.Models.StageModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/stage")]
    public class StageController : LocallyCachedController<int, Stage, StageStubModel>
    {
        public StageController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(DeletedItemType.Stage, dbContext, autoMapper, throttlingService)
        {
        }

        protected override IQueryable<Stage> GetNewItems(IQueryable<Stage> entities, DateTimeOffset date)
        {
            return entities.Where(x => x.EditedDate > date || x.StageAliases.Any(y => y.EditedDate > date) || x.Teams.Any(y => y.EditedDate > date) || x.InvasionTeams.Any(y => y.EditedDate > date));
        }

        protected override IQueryable<DateTimeOffset?> GetTimeStamps(IQueryable<Stage> entities)
        {
            return entities.SelectMany(x => x.StageAliases.Select(y => y.EditedDate)
                .Concat(x.Teams.Select(y => y.EditedDate))
                .Concat(x.InvasionTeams.Select(y => y.EditedDate))
                .Concat(new[] { x.EditedDate })
            );
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
                    Global = live[true],
                    Japan = live[false]
                },
                Upcoming = new ScheduleSubModel
                {
                    Global = upcoming[true],
                    Japan = upcoming[false]
                },
            };
            return Ok(results);
        }
    }
}
