using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.StageModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/stage")]
    public class StageController : LocallyCachedController<int, Stage, StageStubModel>
    {
        public StageController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override IQueryable<Stage> GetNewItems(IQueryable<Stage> entities, DateTimeOffset date)
        {
            return entities.Where(x => x.EditedDate > date || x.StageAliases.Any(y => y.EditedDate > date) || x.Teams.Any(y => y.EditedDate > date) || x.InvasionTeams.Any(y => y.EditedDate > date));
        }

        protected override DateTimeOffset? GetTimeStamp(IQueryable<Stage> entities)
        {
            return entities.SelectMany(x => x.StageAliases.Select(y => y.EditedDate)
                .Concat(x.Teams.Select(y => y.EditedDate))
                .Concat(x.InvasionTeams.Select(y => y.EditedDate))
                .Concat(new[] { x.EditedDate })
            ).Where(x => x != null).Max();
        }
    }
}
