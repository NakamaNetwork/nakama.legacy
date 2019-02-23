using Microsoft.AspNetCore.Mvc;
using NakamaNetwork.Entities;
using TreasureGuide.Web.Controllers.API.Generic;
using TreasureGuide.Web.Services;

namespace TreasureGuide.Web.Controllers.API
{
    [Route("api/ship")]
    public class ShipController : LocallyCachedController
    {
        public ShipController(TreasureEntities dbContext, IThrottleService throttlingService) : base(CacheItemType.Ship, dbContext, throttlingService)
        {
        }
    }
}
