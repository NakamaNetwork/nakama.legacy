using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TreasureGuide.Entities;
using TreasureGuide.Entities.Helpers;
using TreasureGuide.Web.Constants;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Models.UnitModels;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/unit")]
    public class UnitController : LocallyCachedController<int, Unit, UnitStubModel>
    {
        public UnitController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(dbContext, autoMapper, throttlingService)
        {
        }

        protected override IQueryable<Unit> GetNewItems(IQueryable<Unit> entities, DateTimeOffset date)
        {
            return entities.Where(x => x.EditedDate > date || x.UnitAliases.Any(y => y.EditedDate > date) || x.EvolvesTo.Any(y => y.EditedDate > date) || x.EvolvesFrom.Any(y => y.EditedDate > date));
        }
    }
}
