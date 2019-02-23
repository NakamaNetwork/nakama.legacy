using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NakamaNetwork.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/unit")]
    public class UnitController : LocallyCachedController
    {
        public UnitController(TreasureEntities dbContext, IMapper autoMapper, IThrottleService throttlingService) : base(CacheItemType.Unit, dbContext, throttlingService)
        {
        }
    }
}
